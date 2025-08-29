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

    public event Action OnArrowFired; // �ִϸ��̼� �̺�Ʈ���� ȣ��� �̺�Ʈ

    private void Awake()
    {
        // ���ӸŴ����� _archer�� �ڽ��� �ν��Ͻ�
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
        Health.OnDie += OnDie; // Start��, �� ��ũ��Ʈ�� OnDie �Լ��� Health��ũ��Ʈ�� OnDie �̺�Ʈ�� ����
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();

        // ���� �ð����� �ֺ� ���� Ž��
        if (Time.frameCount % 120 == 0) // 120�����Ӹ��� (�뷫 2��) �� ���� ����
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

        // ��ũ��Ʈ ��Ȱ��ȭ
        enabled = false;

        // ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ
        Controller.enabled = false;

        UIManager.Instance.ShowGameOverUI();
    }

    // Animator �̺�Ʈ���� ȣ���� �Լ�
    public void FireArrowEvent()
    {
        OnArrowFired?.Invoke();
    }
}