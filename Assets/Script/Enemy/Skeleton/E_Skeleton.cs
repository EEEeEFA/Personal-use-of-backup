using UnityEngine;

public class E_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }    
    #endregion

    [SerializeField] public float battleTime;

    protected override void Awake()
    {
        base.Awake();

        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this,stateMachine,"Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialized(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public virtual RaycastHit2D isPlayerDectected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, setPlayer);

    public override bool StunCheck()
    {
        if(base.StunCheck())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        else 
            return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
