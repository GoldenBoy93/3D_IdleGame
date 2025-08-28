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

    [field: SerializeField] public Weapon Weapon { get; private set; }

    public Health Health { get; private set; }

    private Action<GameObject> returnToPool; // Ǯ�� ��ȯ�� �� ȣ��Ǵ� �ݹ� �Լ�

    private void Awake()
    {
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
    }
}