using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolable
{
    [field: Header("Reference")]

    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public WarriorAnimationData AnimationData { get; private set; }

    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private EnemyStateMachine stateMachine;

    [field: SerializeField] public Weapon Weapon { get; private set; }

    public Health Health { get; private set; }

    EnemyManager enemyManager;

    private Action<GameObject> returnToPool; // Ǯ�� ��ȯ�� �� ȣ��Ǵ� �ݹ� �Լ�

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

        // ��ũ��Ʈ ��Ȱ��ȭ
        enabled = false;

        // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        Controller.enabled = false;

        StartCoroutine(DelayDie());
    }

    // �� ��� �ִϸ��̼��� ���� �����ֱ� ���� �ð�����
    private IEnumerator DelayDie()
    {
        yield return new WaitForSeconds(5f);
                
        enemyManager.RemoveEnemyOnDeath(this); // ����ִ� �� List���� ����� ��ü ������ ���� ���ִ� �Լ� ȣ��

        OnDespawn(); // ������ƮǮ �������� ���� �Լ� ȣ��
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction; // ��ȯ �ݹ� ����
    }

    public void OnSpawn()
    {
        // �Ŵ��� �ν��Ͻ� ���� Ȯ��
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.Instance;
        }

        // 1. Health ������Ʈ �ʱ�ȭ �� �̺�Ʈ ����
        Health.InitHealth(); // Health ��ũ��Ʈ�� �� �޼��带 �߰��ؾ� ��
        Health.OnDie += OnDie;

        // 2. ���� ���� �缳��
        // Animator.SetBool�� ����Ͽ� "Die" ���¸� �ʱ�ȭ
        //Animator.SetBool(AnimationData.DieParameterHash, false);
        // ���� �ӽ��� �ʱ� ���·� ����
        stateMachine.ChangeState(stateMachine.IdleState);

        // 3. ��ũ��Ʈ�� ��Ʈ�ѷ� Ȱ��ȭ
        enabled = true;
        Controller.enabled = true;
    }

    public void OnDespawn()
    {
        // Health �̺�Ʈ ���� ����
        Health.OnDie -= OnDie;

        returnToPool?.Invoke(gameObject); // Ǯ�� ��ȯ
    }
}