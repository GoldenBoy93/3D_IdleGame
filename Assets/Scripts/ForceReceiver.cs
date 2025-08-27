using UnityEngine;

// Rigidbody���� ����ߴ� �߷�, AddForce�� ������� ���� ����� ���
// Rigidbody�� velocity�� ������ش� �����ϸ� �����ϴ�.
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [SerializeField] private float drag = 0.3f; // ���ݽ� �ణ �̵��� �� �ֵ��� 

    // �����̵� ��ȭ��
    private float verticalVelocity;

    // ����*���� �̵� ��ȭ���� Movement ������ ���� ���� ��
    // ����� ���� �̵��� ���� (���� ��)
    // �Ʒ� Update������ ���� �پ� ���� ���� �׷��� ���� �� verticalVelocity ��ȭ��
    // �ܺο��� Movement ������ �� Update�� ���� verticalVelocity�� ������.
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
            // ���� �پ� ���� ���� �߷��� ���� ��, gravity.y (-9.8)�� �״�� ����
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            // ������ �������� ���� gravity.y (-9.8)�� �������Ѽ� y �������� ����
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