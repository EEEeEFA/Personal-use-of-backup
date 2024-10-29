using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed * .8f, player.rb.velocity.y);

        if (player.rb.velocity.y < 0 )
            stateMachine.ChangeState(player.airState);

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.slideState);
            Debug.Log("Jump to slide works");

        }

    }
}
