using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ThunderEffectController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            Debug.Log("À×Ãù");
            Enemy _enemy = collision.GetComponent<Enemy>();
            EnemyStats _enemyStats = collision.GetComponent<EnemyStats>();

            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();


            playerStats.DoMagicDamage(_enemyStats, _enemy);

        }


    }
}
