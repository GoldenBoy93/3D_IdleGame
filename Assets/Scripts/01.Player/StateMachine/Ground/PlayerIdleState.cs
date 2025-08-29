using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Ground/Idle�� ����
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f; // Idle �����̱� ������ �������� zero
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    // Ground/Idle���� �ٸ� ���·� �������� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // Idle ���¿����� �Է��� �������� �����ϰ� üũ
        // �Է��� ���Դٸ� (MovementInput�� 0,0 �� �ƴ� �ٸ� ���� ���´ٸ�)
        if (stateMachine.MovementInput != Vector2.zero)
        {
            // �ȴ� ���·� ��ȯ
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}