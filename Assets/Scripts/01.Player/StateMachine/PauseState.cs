using UnityEngine;
using UnityEngine.InputSystem;

public class PauseState : PlayerBaseState
{
    public PauseState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 게임 시간 정지
        Time.timeScale = 0f;

        // 마우스 커서 활성화 및 보이게 하기
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 상점 UI 활성화
        //ShopManager.Instance.ShowShopUI();
    }

    public override void Exit()
    {
        base.Exit();

        // 게임 시간 재개
        Time.timeScale = 1.0f;

        // 마우스 커서 비활성화 및 숨기기
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 상점 UI 비활성화
        //GameManager.Instance.ShopManager.HideShopUI();
    }

    protected override void OnPauseStarted(InputAction.CallbackContext context)
    {
        // 일시정지 상태에서 ESC를 다시 누르면 Idle 상태로 돌아감
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}