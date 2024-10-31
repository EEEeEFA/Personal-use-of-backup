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

            if(Vector2.Distance(transform.position, player.transform.position) < .5f)
                player.ClearSword();
        }
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    public void SetupSword(Vector2 _velocity, float _gravityScale, Player _player, float _returnSpeed)
    {
        player = _player;
        rb.velocity = _velocity;
        rb.gravityScale = _gravityScale;
        speed = _returnSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
