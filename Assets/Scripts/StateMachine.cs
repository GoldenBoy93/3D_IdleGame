// State�鿡 ���������� �ʿ��� �Լ��� ����
public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}

public abstract class StateMachine
{
    protected IState currentState; // �ֱ� State ����

    // State ���� �Լ�
    public void ChangeState(IState state)
    {
        currentState?.Exit(); // ���� State�� ������
        currentState = state; // ���ο� State ������ ����
        currentState?.Enter(); // ���ο� State ����
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    // �����ֱ��Լ� Update�ʹ� �ٸ�, �̸��� �Ȱ��� ����� ���� �Լ���
    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
