using System;
using System.Collections;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [field: Header("Reference")]

    [field: SerializeField] public ArcherSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public ArcherAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private ArcherStateMachine stateMachine;

    public Health Health { get; private set; }

    public event Action OnArrowFired; // 애니메이션 이벤트에서 호출될 이벤트

    private void Awake()
    {
        // 게임매니저의 _archer에 자신을 인스턴스
        GameManager.Instance.Archer = this;

        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<Health>();

        stateMachine = new ArcherStateMachine(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie; // Start시, 이 스크립트의 OnDie 함수를 Health스크립트의 OnDie 이벤트에 구독
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();

        // 일정 시간마다 주변 적을 탐색
        if (Time.frameCount % 120 == 0) // 120프레임마다 (대략 2초) 한 번씩 실행
        {
            stateMachine.FindNearestTarget();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");

        // 스크립트 비활성화
        enabled = false;

        // 캐릭터 컨트롤러 비활성화
        Controller.enabled = false;

        UIManager.Instance.ShowGameOverUI();
    }

    // Animator 이벤트에서 호출할 함수
    public void FireArrowEvent()
    {
        OnArrowFired?.Invoke();
    }
}