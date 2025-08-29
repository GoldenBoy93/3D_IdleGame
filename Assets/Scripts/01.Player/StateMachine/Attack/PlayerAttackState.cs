using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    // 공격 시 힘 적용 여부를 추적하는 플래그
    private bool alreadyApplyForce;

    // 공격 판정 적용 여부를 추적하는 플래그
    private bool alreadyAppliedDealing;

    // 현재 공격의 데이터를 저장할 변수
    private AttackInfoData currentAttackData;

    public PlayerAttackState(PlayerStateMachine StateMachine) : base(StateMachine)
    {
    }

    public override void Enter()
    {
        // 상태 진입 시 이동속도를 0으로 설정
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();

        // 콤보 공격을 위한 플래그들을 초기화
        alreadyApplyForce = false;
        alreadyAppliedDealing = false;

        // 현재 콤보 횟수(인덱스)를 가져와 올바른 공격 데이터를 가져옴
        // 예를 들어, 1번째 공격이면 인덱스는 0, 2번째 공격이면 인덱스는 1.
        int currentAttackIndex = stateMachine.ComboIndex;
        currentAttackData = stateMachine.Player.Data.AttackData.GetAttackInfo(currentAttackIndex);

        // 애니메이션 시작
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

        // 캐릭터에 힘을 적용 (공격시 앞으로 쏠리는 효과)
        ForceMove();

        // 현재 애니메이션의 진행 상황을 0~1 사이의 값으로 가져옴
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        // 애니메이션이 실행 도중일 경우
        if (normalizedTime < 1f)
        {
            // 공격에 힘을 적용할 시점인지 확인하고, 아직 적용하지 않았다면 적용합니다.
            if (normalizedTime >= currentAttackData.ForceTransitionTime)
            {
                TryApplyForce();
            }

            // 무기 콜라이더 활성화
            if (!alreadyAppliedDealing && normalizedTime >= currentAttackData.Dealing_Start_TransitionTime)
            {
                // 무기에 피해량과 힘을 설정하고 활성화
                stateMachine.Player.Weapon.SetAttack(currentAttackData.Damage, currentAttackData.Force);
                stateMachine.Player.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            // 무기 콜라이더 비활성화
            if (alreadyAppliedDealing && normalizedTime >= currentAttackData.Dealing_End_TransitionTime)
            {
                stateMachine.Player.Weapon.gameObject.SetActive(false);
            }
        }
    }

    private void TryApplyForce()
    {
        // 이미 힘을 적용했다면 중복 실행 방지
        if (alreadyApplyForce) return;
        alreadyApplyForce = true;

        // ForceReceiver를 리셋하고
        stateMachine.Player.ForceReceiver.Reset();

        // 현재 공격 데이터에 설정된 힘을 적용
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * currentAttackData.Force);
    }
}