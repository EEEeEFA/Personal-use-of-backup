using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] public float cooldownTime;
    protected float timeCounter;

    protected virtual void Update()
    {
        timeCounter -= Time.deltaTime;
    }


    public virtual bool SkillCoolDownCheck()
    {
        if(timeCounter < 0)
        {
            UseSkill();
            timeCounter = cooldownTime;
            return true;
        }
        Debug.Log("Skill is on cool down");
        return false;
    }

    protected virtual void UseSkill()
    {

    }


}
