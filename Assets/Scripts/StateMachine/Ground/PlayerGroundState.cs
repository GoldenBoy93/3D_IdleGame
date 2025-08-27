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
        // base�� �ִ� �Լ� ȣ��
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

        // ���� 1) ���� �پ����� ������ üũ
        // ���� 2) velocity.y�� gravity.y * Time.fixedDeltaTime ���� �۾����� �� üũ
        // �� �۴ٸ� ForceReceiver���� �����Ǿ ���� ���� ����.
        // 9�� ForceReceiver ���� �ٽ� Ȯ��
        if (!stateMachine.Player.Controller.isGrounded
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            // Fall ���·� ��ȯ
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // cancel ������ �Է� ���� zero�� Ȯ���� X
        if (stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        // cancel �̺�Ʈ�� �߻��ߴٴ� �� ���� 0,0 �� ���ٴ� ��
        // Idle ���·� ��ȯ�� �ʿ�
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    // Jump�� ���� �پ����� �� (Idle, Walk, Run) �����ϹǷ� GroundState���� ����
    // BaseState���� ����� ���� �Լ� override�ؼ� ����
    // �̺�Ʈ ȣ�� �� �� �߻� �� ����
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        // Jump ���·� ��ȯ
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }
}