using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // ���� ī�޶��� Transform�� ã���ϴ�.
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Canvas�� �׻� ī�޶� �ٶ󺸵��� ȸ����ŵ�ϴ�.
        // LookAt() �Լ��� ������ ���(ī�޶�)�� �ٶ󺸰� �մϴ�.
        // Vector3.up�� ������Ʈ�� ���� ������ ������ ���ʰ� ��ġ�ϵ��� �մϴ�.
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                         mainCameraTransform.rotation * Vector3.up);
    }
}