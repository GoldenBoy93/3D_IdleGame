using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [field: Header("Reference")]
    // EnemySO만들고 수정
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private EnemyStateMachine stateMachine;

    [field: SerializeField] public Weapon Weapon { get; private set; }

    public Health Health { get; private set; }

    EnemyManager enemyManager;

    private Action<GameObject> returnToPool; // 풀로 반환할 때 호출되는 콜백 함수

    private void Awake()
    {
        AnimationData.Initialize();

        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<Health>();

        stateMachine = new EnemyStateMachine(this);

        enemyManager = EnemyManager.Instance;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
        enemyManager.RemoveEnemyOnDeath(this); // 살아있는 적 List에서 사망한 객체 본인을 제거 해주는 함수 호출

        OnDespawn();
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction; // 반환 콜백 저장
    }

    public void OnSpawn()
    {
        // 매니저 인스턴스 참조 확인
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.Instance;
        }

        // 1. Health 컴포넌트 초기화 및 이벤트 구독
        Health.InitHealth(); // Health 스크립트에 이 메서드를 추가해야 함
        Health.OnDie += OnDie;

        // 2. 적의 상태 재설정
        // Animator.SetBool을 사용하여 "Die" 상태를 초기화
        //Animator.SetBool(AnimationData.DieParameterHash, false);
        // 상태 머신을 초기 상태로 변경
        stateMachine.ChangeState(stateMachine.IdleState);

        // 3. 스크립트와 컨트롤러 활성화
        enabled = true;
        Controller.enabled = true;
    }

    public void OnDespawn()
    {
        // Health 이벤트 구독 해제
        Health.OnDie -= OnDie;
        // 캐릭터 컨트롤러 비활성화
        Controller.enabled = false;

        returnToPool?.Invoke(gameObject); // 풀로 반환
    }
}