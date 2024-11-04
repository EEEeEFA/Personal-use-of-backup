using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TS_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Player player;
    private Rigidbody2D rb;
    private bool canRotate = true;
    private bool isReturning = false;
    private float speed;

    [Header("Bounce info")]
    public float bouncingSpeed;
    public bool isBouncing;
    public int amountOfBounce;
    public List<Transform> enemyTarget;
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < .5f)
                player.CatchSword();
        }

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bouncingSpeed * Time.deltaTime);

            if (amountOfBounce < 0)
            {
                isBouncing = false;
                isReturning = true;
            }

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                targetIndex++;
                amountOfBounce--;
            }

            if (targetIndex >= enemyTarget.Count)
                targetIndex = 0;
        }
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

        anim.SetBool("Rotation", true);
    }

    public void SetupSword(Vector2 _velocity, float _gravityScale, Player _player, float _returnSpeed)
    {
        player = _player;
        rb.velocity = _velocity;
        rb.gravityScale = _gravityScale;
        speed = _returnSpeed;

        anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bouncingSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        bouncingSpeed = _bouncingSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)//���������˽���
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);

                foreach ( var hit in colliders)//�����Χ�Ƿ��б�ĵ���
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform); 

                }
            }
        }

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        transform.parent = collision.transform;
        anim.SetBool("Rotation", false);
    }
}
