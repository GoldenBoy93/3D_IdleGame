using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field : Header("Animations")]
    [field : SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    
    public Animator Animator { get; private set; }
    public PlayerController Input {  get; private set; }
    public CharacterController Controller { get; private set; }

    void Awake()
    {
        AnimationData.Initialize(); // Hash값 생성
        Animator = GetComponent<Animator>();
        Input = GetComponent<PlayerController>();   
        Controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }
}
