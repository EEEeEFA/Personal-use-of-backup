using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{ 
public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            Debug.Log("ground to air works");

        }

        if (Input.GetKeyDown(KeyCode.Space)&& player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.ChangeState(player.attackState);
            Debug.Log("ground to attack works");
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.defendState);
            Debug.Log("ground to Defend works");
        }


    }
}
