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

    private float maxSize;
    private bool canGrow = true;
    private float growSpeed;
    private float shrinkSpeed;

     private List<Transform> enemyTargets = new List<Transform>();
    private List<Enemy> enemyScanned = new List<Enemy>();
    private List<GameObject> createdHotKeyPrefabs = new List<GameObject>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _attackCoolDownTime, int _attackAmount)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        attackCoolDownTime = _attackCoolDownTime;
       
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;  

        if (Input.GetKeyDown(KeyCode.P))
        {
            DestoryHotKey();

            canAttack = true;

            canCreateHotkey = false;
        }

        if (canGrow && !canShrink)//扩黑洞
        {
            transform.localScale = Vector2.Lerp( transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)//缩黑洞
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if(transform.localScale.x < 0)
            Destroy(this.gameObject);

            for (int i = 0; i < enemyScanned.Count; i++)
            {
                enemyScanned[i].FreezeTime(false);
            }

        }

        if (attackTimer < 0 && canAttack)//按P后开始攻击
        {
            attackTimer = attackCoolDownTime;

            int randomIndex = Random.Range(0, enemyTargets.Count);

            PlayerSkillManager.instance.clone.CreateClone(enemyTargets[randomIndex], new Vector3(2, 0, 0));
            attackAmount--;

            if(attackAmount <= 0)
            {
                canAttack = false;

                canShrink = true;
            }

        }
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

            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (!canCreateHotkey)
            return;

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        createdHotKeyPrefabs.Add(newHotkey);

        KeyCode choosenCode = KeyCodeList[Random.Range(0, KeyCodeList.Count)];
        KeyCodeList.Remove(choosenCode);

        BH_Hotkey_Controller newHotkeyScript = newHotkey.GetComponent<BH_Hotkey_Controller>();

        newHotkeyScript.SetupHotKey(choosenCode, collision.transform, this);
    }

    public void AddEnemyTarget(Transform _enemy)
    {
        enemyTargets.Add(_enemy);
    }
}
