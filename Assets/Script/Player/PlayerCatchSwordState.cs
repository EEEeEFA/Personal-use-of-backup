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
        // ������Ч
        player.fx.StartCoroutine("BlingFX");

        sword = player.sword;
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            // ���ݽ���λ�ú���ҵĳ�������Ƿ�ת
            bool shouldFlip = (sword.transform.position.x < player.transform.position.x && player.facingDir == 1) ||
                              (sword.transform.position.x > player.transform.position.x && player.facingDir == -1);

            if (shouldFlip)
            {
                player.Flip();
            }

            // ������ҵ��ƶ��ٶ�
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
