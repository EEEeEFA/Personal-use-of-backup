using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private float lasttimeAttack;
    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .2f;

        if (Time.time >= lasttimeAttack + player.comboWindow || player.comboCounter > 2)
            player.comboCounter = 0;

        player.anim.SetInteger("comboCounter", player.comboCounter);

        xInput = 0; //we need this to fix bug on attack direction

        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[player.comboCounter].x * attackDir, player.attackMovement[player.comboCounter].y);
    }

    public override void Exit()
    {

        base.Exit();

        player.StartCoroutine("BusyFor", .1f);

        player.comboCounter++;
        lasttimeAttack = Time.time;

    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)//ËÙ¶ÈµÝ¼õ
        {

            Vector2 currentVelocity = player.rb.velocity;

            float decelerationFactor = 0.5f;

            Vector2 newVelocity = currentVelocity * decelerationFactor;

            if (newVelocity.magnitude < 0.15f)
            {
                newVelocity = Vector2.zero;
            }

            player.SetVelocity(newVelocity.x, newVelocity.y);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

    }
}
