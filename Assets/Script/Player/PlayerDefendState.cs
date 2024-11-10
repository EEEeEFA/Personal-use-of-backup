using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefendState : PlayerState
{
    public PlayerDefendState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .2f;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        player.SetVelocity(0, 0);
        base.Update();
        if (triggerCalled)
        {
            stateTimer = 10;
                
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }

        if (stateTimer < 0)
         stateMachine.ChangeState(player.idleState);

    }
}
