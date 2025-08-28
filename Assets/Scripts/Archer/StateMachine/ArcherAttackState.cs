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

        // 공격 상태 진입 시, 이벤트 리스너 추가
        // 애니메이션 이벤트에 연결될 함수를 등록
        stateMachine.Archer.OnArrowFired += OnArrowFire;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Archer.AnimationData.AttackParameterHash);

        // 공격 상태 나갈 때, 이벤트 리스너 해제
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

    // 애니메이션 이벤트에 의해 호출될 함수
    private void OnArrowFire()
    {
        // Raycast를 발사할 시작점을 아처의 눈높이로 설정 (예시: 캐릭터 컨트롤러의 절반 높이)
        Vector3 origin = stateMachine.Archer.transform.position + Vector3.up * (stateMachine.Archer.Controller.height / 2f);

        if (stateMachine.Target == null)
        {
            return;
        }

        // Ray의 방향을 계산. 타겟의 눈높이(또는 활을 쏠 지점)를 고려
        // 타겟의 위치에서 origin을 뺀 벡터를 사용
        Vector3 direction = GetMovementDirection();
        float maxDistance = stateMachine.Archer.Data.AttackRange;

        // Raycast는 발사 순간에 한 번만 실행
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            Debug.Log("화살이 " + hit.collider.gameObject.name + "에 명중했습니다.");
            Debug.DrawRay(origin, direction * hit.distance, Color.red, 2f); // 2초간 빨간색 Ray를 그림
        }
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.green, 2f); // 2초간 초록색 Ray를 그림
        }
    }
}