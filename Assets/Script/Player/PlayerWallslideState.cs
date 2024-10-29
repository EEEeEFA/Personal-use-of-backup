using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallslideState : PlayerState
{
    public PlayerWallslideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }       

    public override void Enter()
    {
        player.SetVelocity(0, 0);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, player.rb.velocity.y * player.slideSpeed);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.walljumpState);
            return;
        }
        if (xInput!= 0 && player.facingDir != xInput)
        stateMachine.ChangeState(player.idleState);

        if (player.IsGroundDetected())
        {
            player.Flip();
            stateMachine.ChangeState(player.idleState);
            Debug.Log("slide to idle works");
        }

        if(!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }


    }
}
