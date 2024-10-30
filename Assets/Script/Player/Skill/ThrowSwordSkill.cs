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
    private Vector2 aimDirection = Vector2.right;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetwenDots;
    [SerializeField] private GameObject dotPrefab; 
    [SerializeField] private Transform dotsParent;
     private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenereateDots();
    }
    protected override void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            for (int i = 0; i < dots.Length; i++)
                dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
        }
    }
    public void CreateSword( )//实例化prefabs,并且把小参数传给 TS_SKILL_CONTROLLER设置prefabs
    {
        launchDir = AimDirection() * launchDir.magnitude;
        GameObject newSword = Instantiate( swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<TS_Skill_Controller>().SetupSword(launchDir, swordGravity);
        aimDirection = Vector2.right;

        DotsActive(false);
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

    public void DotsActive(bool _isActive)//点可视化开关，状态机aim里打开，CreateSword里面关闭
    {
        for(int i = 0;i <dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    public Vector2 AimDirection()//丢剑的方向，按↑旋转
    {
        bool enterChance = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) && !enterChance)
        {
            aimDirection = Quaternion.Euler(0, 0, -5) * aimDirection;
            Debug.Log("Aimdirecet");
            enterChance = true;
            
        }

        return aimDirection;
    }
    private Vector2 DotsPosition(float t)//在本脚本的Update里实现
    {
        Vector2 position = (Vector2)player.transform.position + (AimDirection() * launchDir.magnitude) * t; //+ .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

}
