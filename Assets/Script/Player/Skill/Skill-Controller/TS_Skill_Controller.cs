using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TS_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Player player;
    private Rigidbody2D rb;
    private bool canRotate = true;
    private bool isReturning = false;
    private float speed;

    private float freezeTimeDuration;

    [Header("Bounce info")]
    public float bouncingSpeed;
    public bool isBouncing = false;
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
                DamageAndFrozen(enemyTarget[targetIndex].GetComponent<Enemy>());

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

    public void SetupSword(Vector2 _velocity, float _gravityScale, Player _player, float _returnSpeed, float _freezeTimeDuration)
    {
        player = _player;
        rb.velocity = _velocity;
        rb.gravityScale = _gravityScale;
        speed = _returnSpeed;
        freezeTimeDuration = _freezeTimeDuration;

        anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bouncingSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        bouncingSpeed = _bouncingSpeed;
        Debug.Log("1");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            DamageAndFrozen(enemy);
        }

        SetupBounceTarget(collision);

        StuckInto(collision);

    }
        void DamageAndFrozen(Enemy enemy)
        {
            enemy.DamageEffect(player);
            enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
        }

    private void SetupBounceTarget(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)//剑碰到敌人进入
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);

                foreach (var hit in colliders)//检测周围是否有别的敌人
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);

                }
            }
        }
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
