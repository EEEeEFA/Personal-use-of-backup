using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;

    [Header("Move info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float idleTime;

    [Header("Attack info")]
    [SerializeField] public float attackDistance;
    [SerializeField] protected LayerMask setPlayer;

    [Header("Stunned info")]
    public float stunnedTime;
    [SerializeField] public Vector2 StunDistance;
    public bool stunCheck;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }


    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public virtual void StunWindowOpen() => stunCheck = true;

    public  virtual void StunWindowClose() => stunCheck = false;

    public virtual bool StunCheck()
    {
        if (stunCheck)
            return true;
        else
            return false;
    }


    public virtual void AnimationTriggerCalled() => stateMachine.currentState.AnimationTriggerCalled();

}
