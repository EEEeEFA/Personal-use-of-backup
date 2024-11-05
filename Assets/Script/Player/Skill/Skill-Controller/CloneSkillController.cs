using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CloneSkillController : MonoBehaviour, Entity.IAttacker
{
    private float cloneTimer;
    private float cloneLossingSpeed = 1;

    private SpriteRenderer sr;

    Transform closestEnemy;

    Animator anim;
    [Header("Attack info")]
    float attackCheckRadius = 0.98f;
    [SerializeField] Transform setAttackP;
    [SerializeField] public int facingDir { get; private set; } = 1;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        closestEnemyCheck();

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * cloneLossingSpeed));
            if (sr.color.a <= 0)
                Destroy(gameObject);
        }

    }

    public void SetupClone(Transform _newtransform, float _cloneDuration, Vector3 _offset)
    {
        transform.position = _newtransform.position + _offset;
        cloneTimer = _cloneDuration;

        if (canAttack())
        {
            anim.SetInteger("AttackNumbers", UnityEngine.Random.Range(1, 3));
        }
    }

    private void closestEnemyCheck()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

        }
        if(closestEnemy.position.x < transform.position.x)
        {
            facingDir = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    private bool canAttack()
    {
        return true;
    }

    private void AnimationTrigger()
    {
        cloneTimer = -1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(setAttackP.position, attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage(this);
        }
    }
}
