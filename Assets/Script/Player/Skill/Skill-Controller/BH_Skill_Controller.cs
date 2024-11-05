using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BH_Skill_Controller : MonoBehaviour
{
    [SerializeField] GameObject hotkeyPrefab;
    [SerializeField] List<KeyCode> KeyCodeList;

    [SerializeField] public int attackAmount;
    [SerializeField] public float attackTimer;
    [SerializeField] public float attackCoolDownTime;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool canShrink = false;

    [SerializeField] private float maxSize;
    [SerializeField] private bool canGrow;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    [SerializeField] public List<Transform> enemyTargets = new List<Transform>();
    [SerializeField] public List<GameObject> createdHotKeyPrefabs = new List<GameObject>();

    private void Update()
    {
        attackTimer -= Time.deltaTime;  

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp( transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if(transform.localScale.x < 0)
            Destroy(this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DestoryHotKey();

            canAttack = true;
        }

        if (attackTimer < 0 && canAttack)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotkey(collision);
        }
    }

    private void CreateHotkey(Collider2D collision)
    {
        if (canShrink)
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
