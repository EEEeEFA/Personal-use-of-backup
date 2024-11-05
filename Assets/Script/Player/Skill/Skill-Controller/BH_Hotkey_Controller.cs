using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BH_Hotkey_Controller : MonoBehaviour
{
    KeyCode hotKey;
    TextMeshProUGUI text;

    private Transform Enemy;
    private BH_Skill_Controller blackHole;

    public void SetupHotKey(KeyCode _hotKey, Transform _Enemy, BH_Skill_Controller _BlackHole)
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        this.Enemy = _Enemy;
        this.blackHole = _BlackHole;

        hotKey = _hotKey;
        text.text = hotKey.ToString();
    }
    void Update()
    {
        if(Input.GetKeyDown(hotKey))
        {
            blackHole.AddEnemyTarget(Enemy);
            Destroy(gameObject);
        }

    }
}
