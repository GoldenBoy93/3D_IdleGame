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

    // ���� ������ Idle�� ��ȯ�ؾ��ϱ� ������ Update���� üũ
    public override void Update()
    {
        base.Update();

        // ���� ��Ҵٸ�
        if (stateMachine.Player.Controller.isGrounded)
        {
            // Idle�� ��ȯ
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
