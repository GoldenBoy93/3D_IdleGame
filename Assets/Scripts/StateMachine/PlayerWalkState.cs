using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Walk 상태로 전환되었을 때
    public override void Enter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        // Idle에서 속도를 0으로 해놨기때문에 다시 속도 값을 넣어줌
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        // Walk 애니메이션으로 전환
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    // Walk 상태에서 Run Action이 start 될 때 실행될 로직 작성
    // BaseState의 AddInputActionsCallbacks 에서 등록해줌
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        // Run 상태로 전환
        stateMachine.ChangeState(stateMachine.RunState);
    }
}
