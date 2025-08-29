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

        // ���� �ð� ����
        Time.timeScale = 0f;

        // ���콺 Ŀ�� Ȱ��ȭ �� ���̰� �ϱ�
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ���� UI Ȱ��ȭ
        //ShopManager.Instance.ShowShopUI();
    }

    public override void Exit()
    {
        base.Exit();

        // ���� �ð� �簳
        Time.timeScale = 1.0f;

        // ���콺 Ŀ�� ��Ȱ��ȭ �� �����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ���� UI ��Ȱ��ȭ
        //GameManager.Instance.ShopManager.HideShopUI();
    }

    protected override void OnPauseStarted(InputAction.CallbackContext context)
    {
        // �Ͻ����� ���¿��� ESC�� �ٽ� ������ Idle ���·� ���ư�
        stateMachine.ChangeState(stateMachine.IdleState);
    }
}