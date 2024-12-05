using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager instance;

    #region Skill
    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public ThrowSwordSkill TS { get; private set; }
    public BlackHoleSkill BH { get; private set; }
    #endregion
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // 保持实例在场景切换时不被销毁
        }
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        TS = GetComponent<ThrowSwordSkill>();
        BH = GetComponent<BlackHoleSkill>();
    }


}
