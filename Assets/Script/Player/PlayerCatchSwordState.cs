using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    GameObject sword;
    
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 启动特效
        player.fx.StartCoroutine("BlingFX");

        sword = player.sword;
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            // 根据剑的位置和玩家的朝向决定是否翻转
            bool shouldFlip = (sword.transform.position.x < player.transform.position.x && player.facingDir == 1) ||
                              (sword.transform.position.x > player.transform.position.x && player.facingDir == -1);

            if (shouldFlip)
            {
                player.Flip();
            }

            // 设置玩家的移动速度
            player.rb.velocity = new Vector2(5 * -player.facingDir, player.rb.velocity.y);


        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);

    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
