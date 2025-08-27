using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce;
        stateMachine.Player.ForceReceiver.Jump(stateMachine.JumpForce);
        base.Enter();
        // base에 있는 함수 호출
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // Jump 상태에서 ForceReceiver에 의해 추락하고 있을 때
        // velocity(변화량)은 음수가 되는 현상을 이용
        if (stateMachine.Player.Controller.velocity.y <= 0)
        {
            // Idle 상태로 전환 → 추후 Fall 상태로 전환 수정
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
