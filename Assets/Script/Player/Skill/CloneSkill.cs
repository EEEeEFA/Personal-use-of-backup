using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    public void CreateClone(Transform _clonetransform)
    {
        GameObject newclone = Instantiate(clonePrefab);
        newclone.GetComponent<CloneSkillController>().SetupClone(_clonetransform, cloneDuration);
    }
}
