using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BH_Skill_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotkeyPrefab;
    [SerializeField] List<KeyCode> KeyCodeList;

    private int attackAmount;
    private float attackTimer;
    private float attackCoolDownTime;
    private bool canAttack = false;
    public bool canShrink = false;
    private bool canCreateHotkey = true;
    private float blackholeDuration ;

    private bool playerCanDisapear = true;

    private float maxSize;
    private bool canGrow = true;
    private float growSpeed;
    private float shrinkSpeed;

    public bool canExitBH {  get; private set; } = false;

     private List<Transform> enemyTargets = new List<Transform>();
    private List<Enemy> enemyScanned = new List<Enemy>();
    private List<GameObject> createdHotKeyPrefabs = new List<GameObject>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _attackCoolDownTime, int _attackAmount, float _blackholeDuration)
    {
        //黑洞技能的参数 maxSize范围 growSpeed扩张速度 shrinkSpped收缩速度 attackAmount攻击次数 attackCoolDownTime攻击间隔 blackholeDuration技能待机时间
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        attackCoolDownTime = _attackCoolDownTime;

        blackholeDuration = _blackholeDuration;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if(blackholeDuration > 0f)
        {
        blackholeDuration -= Time.deltaTime;
        }

        if (blackholeDuration < 0) //超时未选， 退出黑洞技能
        {
            blackholeDuration = Mathf.Infinity;

            if (enemyTargets.Count > 0)//如果选了目标就放技能
                ReleaseCloneAttack();
            else
            FinishBlackHole();//没选目标直接结束技能


        }

        //再次按H释放技能
        if (Input.GetKeyDown(KeyCode.H) && PlayerManager.instance.player.blackHoleState.skillUsed)
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();
    }

    private void ReleaseCloneAttack()
    {
        if(enemyTargets.Count <= 0) 
            return;

        DestoryHotKey();//消除怪物头上的字母

        canAttack = true;

        canCreateHotkey = false;

        if (playerCanDisapear)
        {
            PlayerManager.instance.player.MakeTransprent(true);
            playerCanDisapear = false;
        }
   
    }

    private void CloneAttackLogic()
    {
        if (canGrow && !canShrink)//扩黑洞
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)//缩黑洞 
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(this.gameObject);
            }

            for (int i = 0; i < enemyScanned.Count; i++)
            {
                enemyScanned[i].FreezeTime(false);
            }

        }

        if (attackTimer < 0 && canAttack && attackAmount > 0)//按H后 canAttack设置为true 开始攻击
        {
            attackTimer = attackCoolDownTime;

            int randomIndex = Random.Range(0, enemyTargets.Count);

            float xOffset;//随机选择一个偏移量

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;


            PlayerSkillManager.instance.clone.CreateClone(enemyTargets[randomIndex], new Vector3(xOffset, 0, 0));
            attackAmount--;

            if (attackAmount <= 0)
            {
                Invoke("FinishBlackHole", .5f);
            }

        }

    }
    void FinishBlackHole()
    {
        DestoryHotKey();
        canAttack = false;
        canShrink = true;

        PlayerManager.instance.player.MakeTransprent(false);

        canExitBH = true;
        //PlayerManager.instance.player.ExitBlackHole();

    }

    private void DestoryHotKey()
    {
        if (createdHotKeyPrefabs.Count <= 0)
            return;
        for (int i = 0; i < createdHotKeyPrefabs.Count; i++)
        {
            Destroy(createdHotKeyPrefabs[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//碰到敌人冻结，并创造Hotkey
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemyScanned.Add(enemy);
            enemy.FreezeTime(true);

            CreateHotkey(collision);//创建HotKey
        }
    }

    private void CreateHotkey(Collider2D collision)
    {

        if (KeyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough hot keys");
            return;
        }

        if (!canCreateHotkey)
            return;

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        createdHotKeyPrefabs.Add(newHotkey);

        KeyCode choosenCode = KeyCodeList[Random.Range(0, KeyCodeList.Count)];//从预设置的键位分配给各个敌人
        KeyCodeList.Remove(choosenCode);

        BH_Hotkey_Controller newHotkeyScript = newHotkey.GetComponent<BH_Hotkey_Controller>();

        newHotkeyScript.SetupHotKey(choosenCode, collision.transform, this);//传入键位 敌人位置 黑洞控制脚本信息
    }                                                                       //                  黑洞脚本用来调用AddEnemyTarget

    public void AddEnemyTarget(Transform _enemy)
    {
        enemyTargets.Add(_enemy);
    }
}
