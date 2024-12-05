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

    [Header("������")]
    public Stats strength;//������1������1��������%1����
    public Stats agility;//���ݣ�1������1%���ܺ�%1������
    public Stats intelligence;//������1������1����ǿ�Ⱥ�%1ħ��
    public Stats vitality;//������1������3����ֵ


    [Header("��������")]//offensive stats
    public Stats dealDamage;
    public Stats critChance;//������
    public Stats critPower;//�����˺�,Ĭ��%150



    [Header("��������")]//defensive stats
    public Stats maxHP;
    public Stats armor;//����
    public Stats evasion;//����
    public Stats magicResistance;//ħ��


    [Header("ħ������")]//magic stats
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    public bool isIgnited;//�Ƿ�ȼ��,�����˺�
    public bool isChilled;//�Ƿ񶳽ᣬ��������20%
    public bool isShocked;//�Ƿ�е磬����������20%

    [SerializeField] private float ailmentsDuration = 4;//�쳣״̬����ʱ��
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;



    private float igniteDamageCoolDown = .3f;//ȼ���˺����ʱ��
    private float igniteDamageTimer;//ȼ���˺���ʱ��
    private int igniteDamage;//ȼ���˺�
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
        ignitedTimer -= Time.deltaTime;//ȼ��ʱ�����
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;//ȼ���˺���ʱ������


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


    private IEnumerator StatModCoruntine(int _modifier, float _duration, Stats _statToModify)//��buff��Э��
    {
        _statToModify.AddModifier(_modifier);//���һ��buff

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);//�Ƴ�һ��buff
    }

    public virtual void DoDamage(CharacterStats _targetStats, Entity _beAttacked)//(�����˺�)if A��B ���������A��ɵ��˺��ܺͺ� ����B��TakeDamage
    {
        //���ܼ���
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        //��������
        if (CirtCheck())
        {
            _totalDamage = CalculateCriticalDamage(_totalDamage);
        }
        //���ܼ���
        _totalDamage = CalculateArmor(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _beAttacked);
        DoMagicDamage(_targetStats, _beAttacked);
    }

    #region Magic Damage and ailments

    public virtual void DoMagicDamage(CharacterStats _targetStats, Entity _beAttacked)//ħ������
    {

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();



        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage, _beAttacked);//����ɵĸ����̳�CharacterStats����


        //important
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)//���Ա�֤�����whileѭ����������ѭ��
            return;


        AttempToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);

    }

    private void AttempToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        //�ж�ħ���˺�����
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;



        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            //����ifͬʱ�жϴ�С���������һ����������˺���boss
            if (UnityEngine.Random.value < .33f && _fireDamage > 0)//Random.value��������һ������ 0.0 �� 1.0 ֮������������
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

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)//Ӧ���쳣״̬
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;//û�������쳣״̬���ܽ���һ���쳣״̬



        //if (isIgnited || isChilled || isShocked)//�������һ���쳣״̬�Ͳ��ܽ�������״̬��
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

            float slowPercentage = .2f;//���ٰٷֱ�

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);//����20%
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
                if (GetComponent<Player>() != null)//��ֹ���˴�����Լ�����
                    return;


                HitNearsetTargerWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)//�Ѿ�����е�Ͳ����ٴν���
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearsetTargerWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//��ײ������Χ�ĵ���

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)//����ǵ��˲��Ҳ����Լ�����ֹ�Լ�����
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


        //Ѱ������ĵ���Ȼ���׻�
        //if (closestEnemy != null)
        //{
        //    GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

        //    newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        //}
    }//���繥��������Ŀ��

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
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//����ȼ���˺�

    public void SetupShockDamage(int _damage) => shockDamage = _damage;//���øе��˺�
 
    #endregion


    #region �˺�����
    public virtual void TakeDamage(int _takeDamage,Entity _beAttacked)//�����˴���
    {
        DecreaseHealthBy(_takeDamage);//Ѫ������
                                      //�ܻ�Ч�� ��ΪҪ����ӿڣ��������ڸ����˺���trigger����


        if (currentHP <= 0 && !Dead)
        {   
            Dead = true;
            Die(_beAttacked);
        }
    }

    protected virtual void Die(Entity _beAttacked)
    {
        _beAttacked.Die();//�л�״̬����deadState,ÿ��ʵ��������Ӹ�Die����
                          //���õ���Ʒ������� �����������津��
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

    #region �����˺�����

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)//����ж�
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

    private int CalculateArmor(CharacterStats _target, int _totalDamage)//���ܼ����˺�
    {
        if (_target.isChilled)
            _totalDamage -= Mathf.RoundToInt(_target.armor.GetValue() * .8f);//��ȥ�Է��Ļ���ֵ
        else
            _totalDamage -= _target.armor.GetValue();//��ȥ�Է��Ļ���ֵ

        _totalDamage =  Mathf.Clamp(_totalDamage, 0, int.MaxValue);//��ֹ���˺����ָ�ֵ
        return _totalDamage;
    }

    private bool CirtCheck()//�����ж�
    {
        int totalCirticalChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= totalCirticalChance)
        {
            return true;
        }
        else
            return false;
    }

    private int CalculateCriticalDamage(int _damage)//���˼���
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)//����ħ��
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);//��ȥħ��ֵ
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);//Clamp����Ѫ����ֵ
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
            return statFunc(); // ���� Lambda ���ʽ����������ֵ
        }

        throw new ArgumentException($"û���Stat��: {_StatType}");
    }
}
