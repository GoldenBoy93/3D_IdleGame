using UnityEngine;

// Rigidbody���� ����ߴ� �߷�, AddForce�� ������� ���� ����� ���
// Rigidbody�� velocity�� ������ش� �����ϸ� �����ϴ�.
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    // �����̵� ��ȭ��
    private float verticalVelocity;

    // ����*���� �̵� ��ȭ���� Movement ������ ���� ���� ��
    // ����� ���� �̵��� ���� (���� ��)
    // �Ʒ� Update������ ���� �پ� ���� ���� �׷��� ���� �� verticalVelocity ��ȭ��
    // �ܺο��� Movement ������ �� Update�� ���� verticalVelocity�� ������.
    public Vector3 Movement => Vector3.up * verticalVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // ���� �پ� ���� ���� �߷��� ���� ��, gravity.y (-9.8)�� �״�� ����
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // ������ �������� ���� gravity.y (-9.8)�� �������Ѽ� y �������� ����
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}