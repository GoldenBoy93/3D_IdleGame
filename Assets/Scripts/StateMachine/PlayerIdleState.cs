using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Ground/Idle로 진입
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f; // Idle 상태이기 때문에 움직임을 zero
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    // Ground/Idle에서 다른 상태로 빠져나갈 때
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // Idle 상태에서는 입력이 들어오는지 꾸준하게 체크
        // 입력이 들어왔다면 (MovementInput에 0,0 이 아닌 다른 값이 들어온다면)
        if (stateMachine.MovementInput != Vector2.zero)
        {
            // 걷는 상태로 전환
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}