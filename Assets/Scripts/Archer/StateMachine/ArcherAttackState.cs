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
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Archer.AnimationData.AttackParameterHash);
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

            // Raycast�� �߻��� �������� ������ ����
            Vector3 origin = stateMachine.Archer.transform.position; // ��ó ���ӿ�����Ʈ ��ġ ��������
            Vector3 direction = GetMovementDirection(); // Enemy ���� (Vector)
            float maxDistance = stateMachine.Archer.Data.AttackRange; // ���� �ִ� �Ÿ�

            // Raycast�� �߻��ϰ�, �浹�� �߻��ߴ��� Ȯ��
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
            {
                // �浹�� �߻����� ���� ����
                Debug.Log("ȭ���� " + hit.collider.gameObject.name + "�� �����߽��ϴ�.");

                // �浹 ������ ��ġ�� ���𰡸� �����ϰų� ȿ���� �� �� �ֽ��ϴ�.
                // Debug.DrawRay�� �� �信�� Ray�� �ð������� �����ݴϴ�. ���� ���ӿ��� ������ ���� �ʽ��ϴ�.
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
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
}