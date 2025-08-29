using UnityEngine;

// ����Ƽ �����Ϳ��� ���� ������ �� �ֵ��� �޴� ������ �߰�
[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemPrice;
    public Sprite itemIcon;

    // ������ ȿ���� �����ϴ� �߻�ȭ�� �Լ�
    public virtual void ApplyEffect()
    {
        Debug.Log("�� �������� ȿ���� ���ǵ��� �ʾҽ��ϴ�.");
    }
}