
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stats strength;     // 1 point increases damage by 1 and crit.power by 1%
    public Stats agility;      // 1 point increases evasion by 1% and crit.chance by 1%
    public Stats intelligence; // 1 point increases magic damage by 1 and magic resistance by 3
    public Stats vitality;     // 1 point increases health by 3 or 5 points

    [Header("Attack stats")]
    public Stats dealDamage;
    public Stats critChance;
    public Stats critPower;

    [Header("Defensive stats")]
    public Stats maxHP;
    public Stats armor;
    public Stats evasion;



    public int currentHP;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        currentHP = GetMaxHealthValue();
    }

    public virtual void DoDamage(CharacterStats _target, Entity _beAttacked)//if A打B ，这里计算A造成的伤害总和后 调用B的TakeDamage
    {
        if (TargetCanAvoidAttack(_target))
        {
            return;
        }
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        //if (CirtCheck())
        //{

        //}

        _totalDamage = CalculateArmor(_target, _totalDamage);
        Debug.Log("DoDamage=" + _totalDamage);

        _target.TakeDamage(_totalDamage, _beAttacked);
    }
    public virtual void TakeDamage(int _takeDamage,Entity _beAttacked)//被打了触发
    {
        DecreaseHealthBy(_takeDamage);//血量计算
                                      //受击效果 因为要传入接口，单独放在各个伤害的trigger那了
        Debug.Log(gameObject.name +_takeDamage);


        if (currentHP <= 0)
        {
            Die(_beAttacked);
        }
    }

    protected virtual void Die(Entity _beAttacked)
    {
        _beAttacked.Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHP -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }

    #region Attack Result Detection

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)//躲避判定
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CalculateArmor(CharacterStats _target, int _totalDamage)
    {
       _totalDamage -= _target.armor.GetValue();

        _totalDamage =  Mathf.Clamp(_totalDamage, 0, int.MaxValue);

        return _totalDamage;
    }

    private bool CirtCheck()
    {
        int totalCirticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCirticalChance)
        {
            return true;
        }
        else
            return false;
    }

    #endregion

    #region finalStats calculate
    public int GetMaxHealthValue()
    {
       return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    //public int GetFinalDamageValue()
    //{

    //}
    #endregion
}
