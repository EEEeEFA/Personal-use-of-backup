using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    public bool skillUsed;
    float flyTime = 1;
    private float defaultgravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = flyTime;

        defaultgravity = player.rb.gravityScale;
        player.rb.gravityScale = 0;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultgravity;
    }

    public override void Update()
    {
        base.Update();
         
        if (stateTimer > 0)
        {
            //Vector3 targetPosition = player.transform.position + new Vector3(0, 15, 0); // 目标位置：正上方15单位
            //float moveSpeed = 15f / 2f; // 两秒内移动15个单位，因此速度是 15 / 2

            //player.transform.position = Vector2.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            player.rb.velocity = new Vector2(0, 10);
        }


        if (stateTimer < 0)
        {
            player.rb.velocity = new Vector2 (0, -.1f);

            if (!skillUsed)
            {
                player.skill.BH.CanUseSkill();
                skillUsed = true;
            }
        }
    }

    //黑洞退出设置在攻击结束时，就是在BH SKILL CONTROLLER里面调用
}
