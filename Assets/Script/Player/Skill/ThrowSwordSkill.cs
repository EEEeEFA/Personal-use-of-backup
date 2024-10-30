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
    public void CreateSword( )//ʵ����prefabs,���Ұ�С�������� TS_SKILL_CONTROLLER����prefabs
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

    public void DotsActive(bool _isActive)//����ӻ����أ�״̬��aim��򿪣�CreateSword����ر�
    {
        for(int i = 0;i <dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    public Vector2 AimDirection()//�����ķ��򣬰�����ת
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
    private Vector2 DotsPosition(float t)//�ڱ��ű���Update��ʵ��
    {
        Vector2 position = (Vector2)player.transform.position + (AimDirection() * launchDir.magnitude) * t; //+ .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

}
