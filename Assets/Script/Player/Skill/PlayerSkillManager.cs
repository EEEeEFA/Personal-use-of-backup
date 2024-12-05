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
            //DontDestroyOnLoad(gameObject); // ����ʵ���ڳ����л�ʱ��������
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
