using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}



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

    [SerializeField] private float ailmentsDuration = 4;//异常状态持续时间
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;



    private float igniteDamageCoolDown = .3f;//燃烧伤害间隔时间
    private float igniteDamageTimer;//燃烧伤害计时器
    private int igniteDamage;//燃烧伤害
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public bool Dead = false;

    public int currentHP;

    public System.Action onHealthChanged;

    private EntityFX fx;

    private Dictionary<StatType, Func<Stats>> statLookup;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHP = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();

        statLookup = new Dictionary<StatType, Func<Stats>>
        {
            { StatType.strength, () => strength },
            { StatType.agility, () => agility },
            { StatType.intelligence, () => intelligence },
            { StatType.vitality, () => vitality },
            { StatType.damage, () => dealDamage },
            { StatType.critChance, () => critChance },
            { StatType.critPower, () => critPower },
            { StatType.health, () => maxHP },
            { StatType.armor, () => armor },
            { StatType.evasion, () => evasion },
            { StatType.magicRes, () => magicResistance },
            { StatType.fireDamage, () => fireDamage },
            { StatType.iceDamage, () => iceDamage },
            { StatType.lightingDamage, () => lightningDamage }
        };
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;//燃烧时间减少
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;//燃烧伤害计时器减少


        if (ignitedTimer < 0)
            isIgnited = false;


        if (chilledTimer < 0)
            isChilled = false;


        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statToModify)
    {
        StartCoroutine(StatModCoruntine(_modifier, _duration, _statToModify));
    }


    private IEnumerator StatModCoruntine(int _modifier, float _duration, Stats _statToModify)//加buff的协程
    {
        _statToModify.AddModifier(_modifier);//添加一个buff

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);//移除一个buff
    }

    public virtual void DoDamage(CharacterStats _targetStats, Entity _beAttacked)//(物理伤害)if A打B ，这里计算A造成的伤害总和后 调用B的TakeDamage
    {
        //闪避计算
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        //暴击计算
        if (CirtCheck())
        {
            _totalDamage = CalculateCriticalDamage(_totalDamage);
        }
        //护盾减免
        _totalDamage = CalculateArmor(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _beAttacked);
        DoMagicDamage(_targetStats, _beAttacked);
    }

    #region Magic Damage and ailments

    public virtual void DoMagicDamage(CharacterStats _targetStats, Entity _beAttacked)//魔法攻击
    {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();



        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage, _beAttacked);//把造成的给到继承CharacterStats的类


        //important
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)//可以保证下面的while循环不会无限循环
            return;


        AttempToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);

    }

    private void AttempToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        //判断魔法伤害类型
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;



        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            //三个if同时判断大小，可以完成一个随机属性伤害的boss
            if (UnityEngine.Random.value < .33f && _fireDamage > 0)//Random.value用于生成一个介于 0.0 和 1.0 之间的随机浮点数
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("ignited" );
                return;
            }

            if (UnityEngine.Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("shocked" );
                return;
            }

            if (UnityEngine.Random.value < .99f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                //Debug.Log("iced" );
                return;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * .1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)//应用异常状态
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;//没有其他异常状态才能进入一个异常状态



        //if (isIgnited || isChilled || isShocked)//如果进入一个异常状态就不能进入其他状态了
        //    return;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;//减速百分比

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);//减速20%
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)//防止敌人打玩家自己被电
                    return;


                HitNearsetTargerWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)//已经进入感电就不如再次进入
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearsetTargerWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//碰撞体检测周围的敌人

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)//如果是敌人并且不是自己，防止自己被电
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;


        }


        //寻找最近的敌人然后雷击
        //if (closestEnemy != null)
        //{
        //    GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

        //    newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        //}
    }//闪电攻击附近的目标

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {

            DecreaseHealthBy(igniteDamage);

            //currentHealth -= igniteDamage;

            if (currentHP < 0 && !Dead)
                Die(GetComponent<Entity>());

            igniteDamageTimer = igniteDamageCoolDown;
        }
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//设置燃烧伤害

    public void SetupShockDamage(int _damage) => shockDamage = _damage;//设置感电伤害
 
    #endregion


    #region 伤害结算
    public virtual void TakeDamage(int _takeDamage,Entity _beAttacked)//被打了触发
    {
        DecreaseHealthBy(_takeDamage);//血量计算
                                      //受击效果 因为要传入接口，单独放在各个伤害的trigger那了


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

    public virtual void IncreaseHealthBy(int _heal)
    {
        currentHP += _heal;

        if (currentHP > GetMaxHealthValue())
            currentHP = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
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

        if (isShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CalculateArmor(CharacterStats _target, int _totalDamage)//护盾减免伤害
    {
        if (_target.isChilled)
            _totalDamage -= Mathf.RoundToInt(_target.armor.GetValue() * .8f);//减去对方的护甲值
        else
            _totalDamage -= _target.armor.GetValue();//减去对方的护甲值

        _totalDamage =  Mathf.Clamp(_totalDamage, 0, int.MaxValue);//防止总伤害出现负值
        return _totalDamage;
    }

    private bool CirtCheck()//暴击判定
    {
        int totalCirticalChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= totalCirticalChance)
        {
            return true;
        }
        else
            return false;
    }

    private int CalculateCriticalDamage(int _damage)//暴伤计算
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)//计算魔抗
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);//减去魔抗值
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);//Clamp限制血量数值
        return totalMagicalDamage;
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

    public Stats GetStat(StatType _StatType)
    {
        if (statLookup.TryGetValue(_StatType, out var statFunc))
        {
            return statFunc(); // 调用 Lambda 表达式，返回属性值
        }

        throw new ArgumentException($"没这个Stat啊: {_StatType}");
    }
}
