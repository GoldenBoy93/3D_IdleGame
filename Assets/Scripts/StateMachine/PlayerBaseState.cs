using UnityEngine;
using UnityEngine.InputSystem;

// ���� ���� Ŭ���� ������ ���������� �ʿ��� ����
public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine StateMachine)
    {
        this.stateMachine = StateMachine;
        groundData = StateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    // �� Action(Movement, Run)�� �̺�Ʈ�� ������ �Լ� ����
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Jump.started += OnJumpStarted;
    }

    // �� Action(Movement, Run)�� �̺�Ʈ�� ���� ������ �Լ� ����
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Jump.started -= OnJumpStarted;
    }

    // Player�� Update���� ��� ȣ��
    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    // Player�� FixedUpdate���� ��� ȣ��
    public virtual void PhysicsUpdate()
    {

    }

    // Player�� Update���� ��� ȣ��
    public virtual void Update()
    {
        Move();
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }

    // �ִϸ��̼� ��ȯ�� �ʿ��� �Լ���
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }

    // Move�� �ʿ��� Vector2���� �޾ƿ��� �Լ�
    private void ReadMovementInput()
    {
        // Generate C# �ɼ� > Player ��ũ��Ʈ�� �Ҵ� �߱� ������ ��ũ��Ʈ�� ���� ������
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    // �����Ӱ� ���õ� ���� (�̵� + ȸ��)
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Move(movementDirection);

        Rotate(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        // ī�޶� �ٶ󺸴� ����� ��ġ ��Ű�� ����
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    // �̵� ����
    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        stateMachine.Player.Controller.Move(((direction * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        // SO�� Ground Data �ۼ��� ���� ����
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }
}