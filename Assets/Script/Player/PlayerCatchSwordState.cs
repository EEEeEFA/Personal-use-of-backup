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
        sword = player.sword;
        if (sword.transform.position.x < player.transform.position.x && player.facingDir == 1)
            player.Flip();
        else if (sword.transform.position.x > player.transform.position.x && player.facingDir == -1)
            player.Flip();

        player.SetVelocity(5 * player.facingDir, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
