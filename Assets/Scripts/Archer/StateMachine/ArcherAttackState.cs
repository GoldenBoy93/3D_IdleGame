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

        // 공격 애니메이션이 재생 중이더라도, 타겟이 사라지거나 거리가 멀어지면 즉시 상태 전환
        if (stateMachine.Target == null || !IsInAttackRange() || stateMachine.Target.IsDie)
        {
            // Debug.Log("놓쳤다");는 타겟이 사라졌을 때 바로 호출될 수 있도록 위치를 옮김
            if (IsInChasingRange()) // 추격 범위에 아직 있다면
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
            }
            else // 추격 범위도 벗어났다면
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            return; // 상태가 변경되었으므로 즉시 함수 종료
        }

        // 기존의 애니메이션 재생 로직은 그대로 유지
        float normalizedTime = GetNormalizedTime(stateMachine.Archer.Animator, "Attack");
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= stateMachine.Archer.Data.ForceTransitionTime)
            {
                TryApplyForce();
            }
        }
        else // 애니메이션이 끝났지만, 타겟이 여전히 공격 범위 내에 있다면 다시 공격 상태로
        {
            stateMachine.ChangeState(stateMachine.AttackState);
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

        Vector3 direction = GetMovementDirection();
        float maxDistance = stateMachine.Archer.Data.AttackRange;

        // Raycast는 발사 순간에 한 번만 실행
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
        {
            Debug.Log("화살이 " + hit.collider.gameObject.name + "에 명중했습니다.");

            if (hit.collider.gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(stateMachine.Archer.Data.Damage);
                Debug.Log($"Damage를 {stateMachine.Archer.Data.Damage}만큼 주었음.");
            }

            Debug.DrawRay(origin, direction * hit.distance, Color.red, 2f); // 2초간 빨간색 Ray를 그림
        }
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, Color.green, 2f); // 2초간 초록색 Ray를 그림
        }
    }
}