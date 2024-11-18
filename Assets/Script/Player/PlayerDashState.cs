using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState :  PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        player.SetVelocity(2*player.dashDir, player.rb.velocity.y);Debug.Log("SetVelocityDashEnd");
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(player.dashDir * player.dashSpeed, 0); Debug.Log("SetVelocityDash");

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.slideState);
            Debug.Log("Dash to slide works");
        }

        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
