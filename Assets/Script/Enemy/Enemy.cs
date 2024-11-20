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
    [SerializeField] public float defaultSpeed;

    [Header("Attack info")]
    [SerializeField] public float attackDistance;
    [SerializeField] protected LayerMask setPlayer;

    [Header("Stunned info")]
    public float stunnedTime;
    [SerializeField] public Vector2 StunDistance;
    public bool stunCheck;


    private string lastAnimBoolName;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultSpeed = moveSpeed;
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

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultSpeed;
    }

    #region TimeFrozen
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            anim.speed = 0;
            moveSpeed = 0;
        }
        else
        {
            anim.speed = 1;
            moveSpeed = defaultSpeed;
        }
    }

    public virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #endregion

    #region stun
    public virtual void StunWindowOpen() => stunCheck = true;

    public  virtual void StunWindowClose() => stunCheck = false;

    public virtual bool StunCheck()
    {
        if (stunCheck)
            return true;
        else
            return false;
    }
    #endregion

    public void AssginLastBoolName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }
    public virtual void AnimationTriggerCalled() => stateMachine.currentState.AnimationTriggerCalled();

}
