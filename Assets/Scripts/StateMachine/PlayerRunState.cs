using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Run 상태로 전환되었을 때
    public override void Enter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        // Run 애니메이션으로 전환
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    // Run 상태에서 다른 상태로 전환 될 때
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
}
