using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private float animationDuration = 0.6f; // 애니메이션의 총 길이
    private float climbDuration = 0.4f; // 올라가는 모션의 길이
    private float startTime;
    private Vector2 startPosition;
    private Vector2 endPosition;

    public PlayerClimbState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        startTime = Time.time;
        startPosition = player.transform.position;
        endPosition = startPosition + new Vector2(1f * player.facingDir, 2f); // 여기서 1은 플레이어가 올라갈 높이입니다. 필요에 따라 조절하세요.

        player.rb.velocity = Vector2.zero;
        player.rb.isKinematic = true;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.isKinematic = false;

    }

    public override void Update()
    {
        base.Update();

        float elapsedTime = Time.time - startTime;

        if (elapsedTime < animationDuration)
        {
            float progress = elapsedTime / climbDuration;
            player.transform.position = Vector2.Lerp(startPosition, endPosition, progress);
        }
        else
            stateMachine.ChangeState(player.idleState);
    }
}
