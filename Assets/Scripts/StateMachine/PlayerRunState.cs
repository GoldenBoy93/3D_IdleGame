using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Run ���·� ��ȯ�Ǿ��� ��
    public override void Enter()
    {
        // BaseSpeed�� ������ �� ����
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        // Run �ִϸ��̼����� ��ȯ
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    // Run ���¿��� �ٸ� ���·� ��ȯ �� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
}
