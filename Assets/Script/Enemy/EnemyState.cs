using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssginLastBoolName(animBoolName);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationTriggerCalled()
    {
        triggerCalled = true;
    }
}
