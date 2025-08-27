using UnityEngine;

// Rigidbody에서 사용했던 중력, AddForce를 사용하지 못해 만드는 기능
// Rigidbody의 velocity를 만들어준다 생각하면 좋습니다.
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float drag = 0.3f; // 공격시 약간 이동할 수 있도록 

    // 수직이동 변화량
    private float verticalVelocity;

    // 수직*수평 이동 변화값은 Movement 변수를 통해 전달 됨
    // 현재는 수직 이동만 구현 (람다 식)
    // 아래 Update문에서 땅에 붙어 있을 때와 그렇지 않을 때 verticalVelocity 변화됨
    // 외부에서 Movement 접근할 때 Update에 따라 verticalVelocity는 유동적.
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private Vector3 dampingVelocity;
    private Vector3 impact;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // 땅에 붙어 있을 때는 중력의 수직 값, gravity.y (-9.8)을 그대로 대입
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // 땅에서 떨어졌을 때는 gravity.y (-9.8)을 누적시켜서 y 포지션을 감소
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    public void Reset()
    {
        verticalVelocity = 0;
        impact = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}