using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwordSkill : Skill
{
    [Header("TS info")]
    [SerializeField] int launchDir;
    [SerializeField] float swordGravity;
    [SerializeField] GameObject swordPrefab;
}
