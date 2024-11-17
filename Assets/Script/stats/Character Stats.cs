
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


    public bool Dead = false;

    public int currentHP;

    public System.Action onHealthChanged;

    protected virtual void Start()
    {
        currentHP = GetMaxHealthValue();
    }

    public virtual void DoDamage(CharacterStats _targetStats, Entity _beAttacked)//if A��B ���������A��ɵ��˺��ܺͺ� ����B��TakeDamage
    {
        //���ܼ���
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }
        int _totalDamage = dealDamage.GetValue() + strength.GetValue();

        //��������
        //if (CirtCheck())
        //{

        //}

        _totalDamage = CalculateArmor(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _beAttacked);
    }

    #region �˺�����
    public virtual void TakeDamage(int _takeDamage,Entity _beAttacked)//�����˴���
    {
        DecreaseHealthBy(_takeDamage);//Ѫ������
                                      //�ܻ�Ч�� ��ΪҪ����ӿڣ��������ڸ����˺���trigger����
        Debug.Log(gameObject.name +_takeDamage);


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

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CalculateArmor(CharacterStats _target, int _totalDamage)//���ܼ����˺�
    {
       _totalDamage -= _target.armor.GetValue();

        _totalDamage =  Mathf.Clamp(_totalDamage, 0, int.MaxValue);//��ֹ���˺����ָ�ֵ

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
