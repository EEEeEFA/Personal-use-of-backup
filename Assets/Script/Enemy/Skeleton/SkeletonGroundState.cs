using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected E_Skeleton enemy;
    Transform player;
    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, E_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        //正面检测到玩家，或玩家在 3（未知单位内）则进入battleState
        if (enemy.isPlayerDectected()||Vector2.Distance(player.transform.position, enemy.transform.position) < 3)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
        //测试按键
        if (Input.GetKeyDown(KeyCode.P))
        {
            stateMachine.ChangeState(enemy.stunnedState);
        }

    }
}
