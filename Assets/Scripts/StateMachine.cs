// State들에 공통적으로 필요한 함수들 정의
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
    protected IState currentState; // 최근 State 상태

    // State 변경 함수
    public void ChangeState(IState state)
    {
        currentState?.Exit(); // 기존 State는 나가고
        currentState = state; // 새로운 State 변수에 저장
        currentState?.Enter(); // 새로운 State 시작
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    // 생명주기함수 Update와는 다른, 이름만 똑같이 만들어 놓은 함수임
    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
