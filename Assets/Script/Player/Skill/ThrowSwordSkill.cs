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
    static Vector2 aimDirection = new Vector2(20, 0);

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab; 
    [SerializeField] private Transform dotsParent;
     private GameObject[] dots;


        public void CreateSword( )
    {
        launchDir = finalDir * launchDir.magnitude;
        GameObject newSword = Instantiate( swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<TS_Skill_Controller>().SetupSword(launchDir, swordGravity);
    }


    public void AimDirection()//放在PlayerAimSwordState里实现了
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            aimDirection.y += 5;
            Debug.Log("Aimdirecet");
        }

        finalDir = aimDirection.normalized;//new Vector2(aimDirection.normalized.x, aimDirection.normalized.y);
    }

    private void GenereateDots()
    {   
        dots = new GameObject[numberOfDots];
        for(int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool _isActive)
    {
        for(int i = 0;i <dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    //private Vector2 DotsPosition(float t)
    //{
    //    Vector2 position = (Vector2)player.transform.position + new Vector2
    //}

}
