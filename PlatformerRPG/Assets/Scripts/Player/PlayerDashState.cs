using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // player.skill.clone.CreateCloen(player.transform);
        stateTimer = player.dashDuration;
        player.cd.enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
        player.cd.enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
