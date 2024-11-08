using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHP;
    public int dealDamage;

    [SerializeField] private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int _takeDamage)//被打了触发
    {
        currentHP -= _takeDamage;

        if (currentHP < 0)
        {
            
        }
    }
}
