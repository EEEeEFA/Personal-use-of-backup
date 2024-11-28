using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BlackHoleSkill : Skill
    {
        [SerializeField] private int amountOfAttacks;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float blackHoleDuration;
        [Space]
        [SerializeField] private GameObject blackHolePrefab;
        [SerializeField] private float maxSize;
        [SerializeField] private float growSpeed;
        [SerializeField] private float shrinkSpeed;
        
        BH_Skill_Controller currentBlackHole;

    public override bool CanUseSkill()
        {
            return base.CanUseSkill();
        }

        public override void UseSkill()
        {

       // Debug.Log("BlackHoleSkill处使用黑洞成功");

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

            currentBlackHole = newBlackHole.GetComponent<BH_Skill_Controller>();

            currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, attackCooldown, amountOfAttacks, blackHoleDuration);
        }

    public bool BlackHoleFinish()
    {
        if(!currentBlackHole) 
            return false;

        if (currentBlackHole.canExitBH)
        
            return true;
  
        else 
            return false;
    }
    }

