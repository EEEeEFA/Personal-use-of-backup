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
    private Vector2 aimDirection;

    [Header("Aim dots")]



    public void CreateSword( )
    {
        GameObject newSword = Instantiate( swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<TS_Skill_Controller>().SetupSword(launchDir, swordGravity);
    }

    public void AimDirection()
    {
        aimDirection = launchDir;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            aimDirection.y += 5;
            Debug.Log("Aimdirecet");
        }

    }
        //finalDir = new Vector2(aimDirection.normalized.x * launchDir.x, aimDirection.normalized.y * launchDir.y);

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2
    }

}
