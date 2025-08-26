using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field : SerializeField] public PlayerSO Data { get; private set; }

    [field : Header("Animations")]
    [field : SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    
    public Animator Animator { get; private set; }
    public PlayerController Input {  get; private set; }
    public CharacterController Controller { get; private set; }

    private PlayerStateMachine stateMachine;

    void Awake()
    {
        AnimationData.Initialize(); // Hash값 생성
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerController>();   
        Controller = GetComponent<CharacterController>();

        stateMachine = new PlayerStateMachine(this);        
    }

    void Start()
    {
        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
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
}
