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

        if(stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 조건 1) 땅에 붙어있지 않은지 체크
        // 조건 2) velocity.y가 gravity.y * Time.fixedDeltaTime 보다 작아지는 지 체크
        // → 작다면 ForceReceiver에서 누적되어서 감소 중인 상태.
        // 9강 ForceReceiver 로직 다시 확인
        if (!stateMachine.Player.Controller.isGrounded
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            // Fall 상태로 전환
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
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

    // Jump는 땅에 붙어있을 때 (Idle, Walk, Run) 가능하므로 GroundState에서 구현
    // BaseState에서 등록해 놓은 함수 override해서 구현
    // 이벤트 호출 될 때 발생 할 로직
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        // Jump 상태로 전환
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }
}