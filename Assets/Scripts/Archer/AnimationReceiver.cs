using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    private Archer archer;

    // Start �Ǵ� Awake���� �θ� ������Ʈ�� Archer ��ũ��Ʈ �ν��Ͻ��� ������
    private void Awake()
    {
        // GetParentsComponent<T>�� �ڽ� ������Ʈ���� �θ� ������Ʈ�� ������Ʈ�� ã�� Ȯ�� �޼����Դϴ�.
        // �Ǵ� transform.parent.GetComponent<Archer>()�� ����� �� �ֽ��ϴ�.
        archer = transform.parent.GetComponent<Archer>();

        if (archer == null)
        {
            Debug.LogError("�θ� ������Ʈ�� Archer ��ũ��Ʈ�� �����ϴ�!");
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �Լ�
    public void FireArrowEvent()
    {
        // �θ� ������Ʈ�� Archer ��ũ��Ʈ�� �ִ� �Լ��� ȣ��
        if (archer != null)
        {
            // Archer ��ũ��Ʈ�� FireArrowEvent() �Լ��� ȣ���մϴ�.
            // Archer ��ũ��Ʈ�� FireArrowEvent() �Լ��� public�̾�� �մϴ�.
            archer.FireArrowEvent();
        }
    }
}