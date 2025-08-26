using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Walk ���·� ��ȯ�Ǿ��� ��
    public override void Enter()
    {
        // BaseSpeed�� ������ �� ����
        // Idle���� �ӵ��� 0���� �س��⶧���� �ٽ� �ӵ� ���� �־���
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        // Walk �ִϸ��̼����� ��ȯ
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    // Walk ���¿��� Run Action�� start �� �� ����� ���� �ۼ�
    // BaseState�� AddInputActionsCallbacks ���� �������
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        // Run ���·� ��ȯ
        stateMachine.ChangeState(stateMachine.RunState);
    }
}
