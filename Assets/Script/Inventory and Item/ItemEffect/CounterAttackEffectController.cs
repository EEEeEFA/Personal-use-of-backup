using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttackEffectController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Debug.Log("shrink");
            Enemy _enemy = collision.GetComponent<Enemy>();
            EnemyStats _enemyStats = collision.GetComponent<EnemyStats>();

            Player _player = PlayerManager.instance.player;

            PlayerStats playerStats = _player.GetComponent<PlayerStats>();


            playerStats.DoDamage(_enemyStats, _enemy);
            _enemy.DamageEffect(_player);

        }
    }
}
