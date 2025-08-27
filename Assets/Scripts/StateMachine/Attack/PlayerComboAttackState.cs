using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedCombo;
    private bool alreadyApplyForce;

    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine StateMachine) : base(StateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false;
        alreadyApplyForce = false;

        int comboindex = stateMachine.ComboIndex;
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboindex);
        stateMachine.Player.Animator.SetInteger("Combo", comboindex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo)
        {
            stateMachine.ComboIndex = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            // ForceTransitionTime (0.1 : �ִϸ��̼� �ʹ�)
            if (normalizedTime >= attackInfoData.ForceTransitionTime)
                TryApplyForce();

            // ComboTransitionTime (0.8 : �ִϸ��̼� ���ڶ�)
            if (normalizedTime >= attackInfoData.ComboTransitionTime)
                TryComboAttack();
        }
        else  // Animation�� ������ �� (normalizedTime�� 1.0�� ��)
        {
            if (alreadyAppliedCombo)
            {
                // ���� ComboAttack ������ ����
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else
            {
                // �޺� ���н� Idle�� (��������)
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }

    private void TryComboAttack()
    {
        if (alreadyAppliedCombo) return;

        if (attackInfoData.ComboStateIndex == -1) return;

        if (!stateMachine.IsAttacking) return;

        alreadyAppliedCombo = true;
    }

    private void TryApplyForce()
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        // ������ �����鼭 ���� ����� ���� �� ����
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }
}
