using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    bool skillUsed;
    float durationTime;
    private float defaultgravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = durationTime;
        defaultgravity = player.rb.gravityScale;

        player.skill.BH.UseSkill();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!canShrink)
        {
            player.rb.gravityScale = 0;
            player.rb.velocity = Vector2.Lerp(player.transform.position, player.transform.position + new Vector3(0 , 10), 4 * Time.deltaTime);
        }

        if (canShrink)
        {
            player.rb.gravityScale = defaultgravity;
        }
    }
}
