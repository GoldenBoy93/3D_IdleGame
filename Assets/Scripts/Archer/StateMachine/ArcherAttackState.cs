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

            // Raycast를 발사할 시작점과 방향을 설정
            Vector3 origin = stateMachine.Archer.transform.position; // 아처 게임오브젝트 위치 가져오기
            Vector3 direction = GetMovementDirection(); // Enemy 방향 (Vector)
            float maxDistance = stateMachine.Archer.Data.AttackRange; // 공격 최대 거리

            // Raycast를 발사하고, 충돌이 발생했는지 확인
            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
            {
                // 충돌이 발생했을 때의 로직
                Debug.Log("화살이 " + hit.collider.gameObject.name + "에 명중했습니다.");

                // 충돌 지점의 위치에 무언가를 생성하거나 효과를 줄 수 있습니다.
                // Debug.DrawRay는 씬 뷰에서 Ray를 시각적으로 보여줍니다. 실제 게임에는 영향을 주지 않습니다.
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