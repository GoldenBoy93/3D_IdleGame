using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    // 땅에 닿으면 Idle로 전환해야하기 때문에 Update에서 체크
    public override void Update()
    {
        base.Update();

        // 땅에 닿았다면
        if (stateMachine.Player.Controller.isGrounded)
        {
            // Idle로 전환
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
