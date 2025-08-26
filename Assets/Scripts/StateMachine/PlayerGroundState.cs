using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // base에 있는 함수 호출
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // cancel 직전에 입력 값이 zero일 확률은 X
        if (stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        // cancel 이벤트가 발생했다는 건 값이 0,0 이 들어간다는 것
        // Idle 상태로 전환이 필요
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }
}