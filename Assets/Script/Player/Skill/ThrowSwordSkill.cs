using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowSwordSkill : Skill
{
    [Header("TS info")]
    [SerializeField] Vector2 launchDir;
    [SerializeField] float swordGravity;
    [SerializeField] GameObject swordPrefab;
    private Vector2 finalDir;

    public void CreateSword( )
    {
        GameObject newSword = Instantiate( swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<TS_Skill_Controller>().SetupSword(launchDir, swordGravity);
    }

    private Vector2 AimDirection()
    {
        Vector2 aimDirection = launchDir;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            aimDirection.y += 5;
        }

        return aimDirection;
    }
}
