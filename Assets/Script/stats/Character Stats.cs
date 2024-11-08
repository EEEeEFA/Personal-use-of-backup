using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats maxHP;
    public Stats dealDamage;
    public Stats strength;

    [SerializeField] private int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP.GetValue();
    }

    public virtual void DoDamage(CharacterStats _target)
    {
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        Debug.Log( "DoDamage=" + _totalDamage);

        _target.TakeDamage(_totalDamage);
    }
    public virtual void TakeDamage(int _takeDamage)//被打了触发
    {
        currentHP -= _takeDamage;
        Debug.Log(gameObject.name +_takeDamage);
        if (currentHP < 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("去世了");
    }
}
