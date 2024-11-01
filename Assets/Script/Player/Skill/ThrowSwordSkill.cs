using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThrowSwordSkill : Skill
{
    [Header("TS info")]
    [SerializeField] Vector2 launchForce;
    [SerializeField] float swordGravity;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float returnSpeed;
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
        launchForce = AimDirectionTemp(player.facingDir) * launchForce.magnitude;


        GameObject newSword = Instantiate( swordPrefab, player.transform.position, transform.rotation);
        newSword.GetComponent<TS_Skill_Controller>().SetupSword(launchForce, swordGravity, player, returnSpeed);

        aimDirection = Vector2.right;

        player.AssignSword(newSword);

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
            float angle = -5f * Mathf.Deg2Rad; // 逆时针旋转5度
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            aimDirection = new Vector2(
                aimDirection.x * cos - aimDirection.y * sin,
                aimDirection.x * sin + aimDirection.y * cos
            );
            enterChance = true;
        }

        return aimDirection.normalized; 

        //// Smoothly tilts a transform towards a target rotation.
        //float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        //float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

        //// Rotate the cube by converting the angles into a quaternion.
        //Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);

    }

    public Vector2 AimDirectionTemp(int playerFacingR)//临时用的
    {

        if (Input.GetAxisRaw("Horizontal") > 0 && playerFacingR <0)
        {
            player.Flip();
         return Vector2.left;
        }
        
        else if (Input.GetAxisRaw("Horizontal") < 0 && playerFacingR > 0)
        {
            player.Flip();
        return Vector2.right;
        }

        return new Vector2(playerFacingR, 0);
    }
    private Vector2 DotsPosition(float t)//在本脚本的Update里实现
    {
        Vector2 position = (Vector2)player.transform.position + (AimDirectionTemp(player.facingDir) * launchForce.magnitude) * t; //+ .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

}
