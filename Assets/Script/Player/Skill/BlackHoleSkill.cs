using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BlackHoleSkill : Skill
    {
        [SerializeField] private int amountOfAttacks;
        [SerializeField] private float attackCooldown;
        [Space]
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;


    public override bool SkillCoolDownCheck()
        {
            return base.SkillCoolDownCheck();
        }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

        public override void UseSkill()
        {
            base.UseSkill();

            GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

            BH_Skill_Controller newBlackHoleScript = newBlackHole.GetComponent<BH_Skill_Controller>();

            newBlackHoleScript.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, attackCooldown, amountOfAttacks);
        }
    }

