
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //[Header("Major stats")]
    //public Stats strength;     // 1 point increases damage by 1 and crit.power by 1%
    //public Stats agility;      // 1 point increases evasion by 1% and crit.chance by 1%
    //public Stats intelligence; // 1 point increases magic damage by 1 and magic resistance by 3
    //public Stats vitality;     // 1 point increases health by 3 or 5 points

    //[Header("Attack stats")]
    //public Stats dealDamage;
    //public Stats critChance;
    //public Stats critPower;

    //[Header("Defensive stats")]
    //public Stats maxHP;
    //public Stats armor;
    //public Stats evasion;

    [Header("主属性")]
    public Stats strength;//力量，1点增加1攻击力和%1爆伤
    public Stats agility;//敏捷，1点增加1%闪避和%1暴击率
    public Stats intelligence;//智力，1点增加1法术强度和%1魔抗
    public Stats vitality;//活力，1点增加3生命值


    [Header("攻击属性")]//offensive stats
    public Stats dealDamage;
    public Stats critChance;//暴击率
    public Stats critPower;//暴击伤害,默认%150



    [Header("防守属性")]//defensive stats
    public Stats maxHP;
    public Stats armor;//护甲
    public Stats evasion;//闪避
    public Stats magicResistance;//魔抗


    [Header("魔法属性")]//magic stats
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    public bool isIgnited;//是否燃烧,持续伤害
    public bool isChilled;//是否冻结，削弱护甲20%
    public bool isShocked;//是否感电，减少命中率20%


    public bool Dead = false;

    public int currentHP;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        currentHP = GetMaxHealthValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats, Entity _beAttacked)//if A打B ，这里计算A造成的伤害总和后 调用B的TakeDamage
    {
        //闪避计算
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        //暴击计算
        //if (CirtCheck())
        //{

        //}

        _totalDamage = CalculateArmor(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _beAttacked);
    }

    #region 伤害结算
    public virtual void TakeDamage(int _takeDamage,Entity _beAttacked)//被打了触发
    {
        DecreaseHealthBy(_takeDamage);//血量计算
                                      //受击效果 因为要传入接口，单独放在各个伤害的trigger那了
        Debug.Log(gameObject.name +_takeDamage);


        if (currentHP <= 0 && !Dead)
        {   
            Dead = true;
            Die(_beAttacked);
        }
    }

    protected virtual void Die(Entity _beAttacked)
    {
        _beAttacked.Die();//切换状态机到deadState,每个实体后续都加个Die方法
                          //骷髅的物品掉落放在 死亡动画里面触发
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHP -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }
    #endregion

    #region 各类伤害计算

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)//躲避判定
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CalculateArmor(CharacterStats _target, int _totalDamage)//护盾减免伤害
    {
       _totalDamage -= _target.armor.GetValue();

        _totalDamage =  Mathf.Clamp(_totalDamage, 0, int.MaxValue);//防止总伤害出现负值

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
