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
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashDir { get; private set; }

    public float defaultMoveSpeed;
    public float defaultJumpForce;
    public float defaultDashSpeed;

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
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }

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

        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "blackHole");

        deadState = new PlayerDeadState(this, stateMachine, "dead");

    }

    protected override void Start()
    {
        base.Start();
        skill = PlayerSkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        //以下为可以随时触发的玩家动作
        CheckDash();
        BHSkill();
        UseFlask();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    #region 可以从任意状态进入的玩家动作
    private void CheckDash()//冲刺
    {
        if (IsWallDetected()) 
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift)&& PlayerSkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);

            skill.clone.CreateClone(transform, Vector3.zero);
        }

    }

    private void BHSkill()//放黑洞技能
    {
        if (Input.GetKeyDown(KeyCode.H) && !blackHoleState.activeBH && skill.BH.OnlyTime())
        {
            stateMachine.ChangeState(blackHoleState);
          
        }
    }

    private void UseFlask()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R");
            Inventory.instance.UseFlask();
        }
    }

    #endregion

    public void AssignSword(GameObject _Sword)
    {
        sword = _Sword;
    }//用来检测是否丢过剑

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        //自己写的
        ChangLayer();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    private void ChangLayer()
    {
        int targetLayer = LayerMask.NameToLayer(DeadGuys) - 1;//使targetLayer作为 DeadGuys 层 的索引，但是我不知道为什么要-1
        Debug.Log(targetLayer);
        // 更改当前 GameObject 的图层
        gameObject.layer = targetLayer;

        // 如果需要递归更改子对象的图层
        SetLayerRecursively(gameObject, targetLayer);
    }

    void SetLayerRecursively(GameObject obj, LayerMask newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
}