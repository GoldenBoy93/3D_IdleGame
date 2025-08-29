using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    // ���� �� �� ���� ���θ� �����ϴ� �÷���
    private bool alreadyApplyForce;

    // ���� ���� ���� ���θ� �����ϴ� �÷���
    private bool alreadyAppliedDealing;

    // ���� ������ �����͸� ������ ����
    private AttackInfoData currentAttackData;

    public PlayerAttackState(PlayerStateMachine StateMachine) : base(StateMachine)
    {
    }

    public override void Enter()
    {
        // ���� ���� �� �̵��ӵ��� 0���� ����
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();

        // �޺� ������ ���� �÷��׵��� �ʱ�ȭ
        alreadyApplyForce = false;
        alreadyAppliedDealing = false;

        // ���� �޺� Ƚ��(�ε���)�� ������ �ùٸ� ���� �����͸� ������
        // ���� ���, 1��° �����̸� �ε����� 0, 2��° �����̸� �ε����� 1.
        int currentAttackIndex = stateMachine.ComboIndex;
        currentAttackData = stateMachine.Player.Data.AttackData.GetAttackInfo(currentAttackIndex);

        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        // ĳ���Ϳ� ���� ���� (���ݽ� ������ �򸮴� ȿ��)
        ForceMove();

        // ���� �ִϸ��̼��� ���� ��Ȳ�� 0~1 ������ ������ ������
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        // �ִϸ��̼��� ���� ������ ���
        if (normalizedTime < 1f)
        {
            // ���ݿ� ���� ������ �������� Ȯ���ϰ�, ���� �������� �ʾҴٸ� �����մϴ�.
            if (normalizedTime >= currentAttackData.ForceTransitionTime)
            {
                TryApplyForce();
            }

            // ���� �ݶ��̴� Ȱ��ȭ
            if (!alreadyAppliedDealing && normalizedTime >= currentAttackData.Dealing_Start_TransitionTime)
            {
                // ���⿡ ���ط��� ���� �����ϰ� Ȱ��ȭ
                stateMachine.Player.Weapon.SetAttack(currentAttackData.Damage, currentAttackData.Force);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            // ���� �ݶ��̴� ��Ȱ��ȭ
            if (alreadyAppliedDealing && normalizedTime >= currentAttackData.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }
        }
    }

    private void TryApplyForce()
    {
        // �̹� ���� �����ߴٸ� �ߺ� ���� ����
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        // ForceReceiver�� �����ϰ�
        stateMachine.Player.ForceReceiver.Reset();

        // ���� ���� �����Ϳ� ������ ���� ����
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * currentAttackData.Force);
    }
}