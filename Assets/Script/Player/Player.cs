using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity;

public class Player : Entity
{
    [Header("Move info")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Dash info")]
    public float DashSpeed = 20f;
    public float DashDuration = 0.15f;
    public float dashDir { get; private set; }

    [Header("OnWall info")]
    public float slideSpeed;

    [Header("Attack info")]
    [SerializeField] public float comboWindow = 2f;
    [SerializeField] public Vector2[] attackMovement;
    [SerializeField] public int comboCounter = 0;


    [Header("Busy info")]
    [SerializeField] public bool isBusy;

    public PlayerSkillManager skill;
    public GameObject sword {  get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }

    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerWallslideState slideState { get; private set; }
    public PlayerWallJumpState walljumpState{ get; private set; }
    public PlayerPrimaryAttack attackState { get; private set; }

    public PlayerDefendState defendState { get; private set; }

    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }

    #endregion

    protected override void Awake()
    {   
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        slideState = new PlayerWallslideState(this, stateMachine, "Slide");
        walljumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        attackState = new PlayerPrimaryAttack(this, stateMachine, "Attack");

        defendState = new PlayerDefendState(this, stateMachine, "Defend");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "SuccessfulCounterAttack");

        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

    }

    protected override void Start()
    {
        base.Start();
        skill = PlayerSkillManager.instance;
        stateMachine.Initialize(idleState);     
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDash();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    } 

    private void CheckDash()
    {
        if (IsWallDetected()) 
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift)&& PlayerSkillManager.instance.dash.SkillCoolDownCheck())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);

            skill.clone.CreateClone(transform, Vector3.zero);
        }

    }

    public void AssignSword(GameObject _Sword)
    {
        sword = _Sword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

}