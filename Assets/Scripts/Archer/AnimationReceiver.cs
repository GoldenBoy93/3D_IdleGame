using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    private Archer archer;

    private void Awake()
    {
        archer = transform.parent.GetComponent<Archer>();

        if (archer == null)
        {
            Debug.LogError("�θ� ������Ʈ�� Archer ��ũ��Ʈ�� �����ϴ�!");
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ���� �Լ�
    public void FireArrowEvent()
    {
        if (archer != null)
        {
            // Archer ��ũ��Ʈ�� FireArrowEvent() �Լ��� ȣ��
            archer.FireArrowEvent();
        }
    }
}