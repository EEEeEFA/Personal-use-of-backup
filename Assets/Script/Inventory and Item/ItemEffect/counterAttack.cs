using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/counterAttack")]
public class counterAttack : itemEffect
{
    [SerializeField]GameObject barbPrefabs;


    //¸¸ÀàÖÐµÄUseEffect
    //public virtual void UseEffect(Transform _enemyTarget)
    //{
    //    Debug.Log("Test Succeed!!!");
    //}
    public override void UseEffect(Transform _enemyTarget)
    {
        Transform _player = PlayerManager.instance.player.transform;
        GameObject newbarbPrefabs = Instantiate(barbPrefabs, _player.position, Quaternion.identity);
        Destroy(newbarbPrefabs, 1f);

    }
}
