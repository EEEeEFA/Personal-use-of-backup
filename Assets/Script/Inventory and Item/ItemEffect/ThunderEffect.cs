using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect ", menuName = "Data/Item Effect/ThunderEffect")]
public class ThunderEffect : itemEffect
{
    [SerializeField] GameObject thunderPrefabs;

    public override void UseEffect(Transform _enemyTarget)
    {
        GameObject newThunderStrike = Instantiate(thunderPrefabs, _enemyTarget.position, Quaternion.identity);
        Destroy(newThunderStrike ,1f);
        //GameObject newThunderStrike = Instantiate(thunderPrefabs, _enemyTarget);
    }
}
