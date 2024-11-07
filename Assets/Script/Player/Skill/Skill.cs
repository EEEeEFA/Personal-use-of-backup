using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public float cooldownTime;
    protected float timeCounter;
    protected Player player;
    protected bool skillUsed = true;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Update()
    {
        timeCounter -= Time.deltaTime;
    }


    public virtual bool SkillCoolDownCheck()//ͨ����ȴʱ���ж��Ƿ��ܹ��ͷż���
    {
        if(timeCounter < 0)
        {
            UseSkill();
            timeCounter = cooldownTime;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }

    public virtual bool CanUseSkill()
    {
        if (timeCounter < 0)
        {
            UseSkill();
            timeCounter = cooldownTime;
            return true;
        }
        else
        {
            Debug.Log("Skill is on cooldown");
            return false;
        }
    }


}
