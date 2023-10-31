using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
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

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    stateMachine.ChangeState(player.aimSword);
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSwrod())
        //{
        //    stateMachine.ChangeState(player.aimSword);
        //}


        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.ChangeState(player.counterAttack);
        }

        if(Input.GetKeyDown(KeyCode.X) && player.stats.currentStamina > 0)
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (player.IsGroundDetected() == false)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.C) && player.IsGroundDetected())
        {
            if (!player.hasJump && !player.hasSecondJump)
            stateMachine.ChangeState(player.jumpState);
            
        }
        
    }

    private bool HasNoSwrod()
    {
        if(!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
