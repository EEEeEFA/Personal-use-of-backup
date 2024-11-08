using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    public bool skillUsed;
    public bool activeBH;
    float flyTime = .5f;

    private float defaultgravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillUsed = false;
        activeBH = true;

        stateTimer = flyTime;

        defaultgravity = player.rb.gravityScale;
        player.rb.gravityScale = 0;

        
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultgravity;
        activeBH = false;
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
                if (player.skill.BH.CanUseSkill())
                {
                    Debug.Log("状态机处使用黑洞成功");
                skillUsed = true;
                }

            }
                if (player.skill.BH.BlackHoleFinish())
                {
                Debug.Log("状态机处退出黑洞");
                stateMachine.ChangeState(player.airState);
                }
        }
    }

}
