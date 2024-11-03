using System.Collections;
using System.Collections.Generic;
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

    public bool isBouncing = true;
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

            if(Vector2.Distance(transform.position, player.transform.position) < .5f )
                player.CatchSword();
        }

        if(isBouncing && enemyTarget.Count >0)
        {
            Debug.Log("right");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)//剑碰到敌人进入
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 30);

                foreach ( var hit in collider)
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
