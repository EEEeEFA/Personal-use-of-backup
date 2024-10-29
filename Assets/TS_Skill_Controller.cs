using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Player player;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupSword(Vector2 _velocity, float _gravityScale)
    {
        rb.velocity = _velocity;
        rb.gravityScale = _gravityScale;
    }
}
