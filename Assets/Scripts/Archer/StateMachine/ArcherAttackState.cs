using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ArcherAttackState : ArcherBaseState
{
    private bool alreadyApplyForce;

    public ArcherAttackState(ArcherStateMachine archerStateMachine) : base(archerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Archer.AnimationData.AttackParameterHash);
        alreadyApplyForce = false;

        // ���� ���� ���� ��, �̺�Ʈ ������ �߰�
        // �ִϸ��̼� �̺�Ʈ�� ����� �Լ��� ���
        stateMachine.Archer.OnArrowFired += OnArrowFire;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Archer.AnimationData.AttackParameterHash);

        // ���� ���� ���� ��, �̺�Ʈ ������ ����
        stateMachine.Archer.OnArrowFired -= OnArrowFire;
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Archer.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= stateMachine.Archer.Data.ForceTransitionTime)
            {
                TryApplyForce();
            }
        }
        else
        {
            if (IsInChasingRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
    }

    private void TryApplyForce()
    {
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        stateMachine.Archer.ForceReceiver.Reset();

        stateMachine.Archer.ForceReceiver.AddForce(stateMachine.Archer.transform.forward * stateMachine.Archer.Data.Force);
    }

    // �ִϸ��̼� �̺�Ʈ�� ���� ȣ��� �Լ�
    private void OnArrowFire()
    {
        // Raycast�� �߻��� �������� ��ó�� �����̷� ���� (����: ĳ���� ��Ʈ�ѷ��� ���� ����)
        Vector3 origin = stateMachine.Archer.transform.position + Vector3.up * (stateMachine.Archer.Controller.height / 2f);

        if (stateMachine.Target == null)
        {
            return;
        }

        // Ray�� ������ ���. Ÿ���� ������(�Ǵ� Ȱ�� �� ����)�� ���
        // Ÿ���� ��ġ���� origin�� �� ���͸� ���
        Vector3 direction = GetMovementDirection();
        float maxDistance = stateMachine.Archer.Data.AttackRange;

        // Raycast�� �߻� ������ �� ���� ����
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            Debug.Log("ȭ���� " + hit.collider.gameObject.name + "�� �����߽��ϴ�.");
            Debug.DrawRay(origin, direction * hit.distance, Color.red, 2f); // 2�ʰ� ������ Ray�� �׸�
        }
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.green, 2f); // 2�ʰ� �ʷϻ� Ray�� �׸�
        }
    }
}