using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.hasJump = true;
        player.SetVelocity(6 * -player.facingDir, player.jumoForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (xInput != 0)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        }

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }

}
