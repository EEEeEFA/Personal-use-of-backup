using UnityEngine;
using UnityEngine.EventSystems;

public class SkeletonBattleState : EnemyState
{
    E_Skeleton enemy;
    Transform player;
    int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, E_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(enemy.isPlayerDectected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.isPlayerDectected().distance < enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.attackState);
                enemy.SetVelocity(0, 0);
                return;
            }
        }

        if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 15)
            stateMachine.ChangeState(enemy.idleState);


        if (player.position.x > enemy.transform.position.x)
        { moveDir = 1; }
        else
        { moveDir = -1; }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);
    }
}
