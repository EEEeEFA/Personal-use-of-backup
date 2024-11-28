using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldownTime;
    [SerializeField] protected float timeCounter;
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

    public virtual bool OnlyTime()//冷却时间到了就返回true
    {
        if (timeCounter < 0)
            return true;
        else return false;
    }
    public virtual bool CanUseSkill()//冷却时间到了则释放技能
    {
        if(OnlyTime())
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


}
