using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Upgrade", menuName = "Shop/Items/Attack Upgrade")]
public class DamageUP : ShopItemSO
{
    public int damageUPAmount = 1;

    public override void ApplyEffect()
    {
        Debug.Log($"{itemName} �������� �����Ͽ� {damageUPAmount}��(��) �������׽��ϴ�.");
        // ���� �÷��̾� ������ ������Ű�� ���� �߰�
        GameManager.Instance.Archer.Data.Damage += damageUPAmount;
        Debug.Log($"���� ���ݷ� {GameManager.Instance.Archer.Data.Damage}");
    }
}