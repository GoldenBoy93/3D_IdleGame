using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Upgrade", menuName = "Shop/Items/Attack Upgrade")]
public class DamageUP : ShopItemSO
{
    public int damageUPAmount = 1;

    public override void ApplyEffect()
    {
        Debug.Log($"{itemName} 아이템을 구매하여 {damageUPAmount}을(를) 증가시켰습니다.");
        // 실제 플레이어 스탯을 증가시키는 로직 추가
        GameManager.Instance.Archer.Data.Damage += damageUPAmount;
        Debug.Log($"현재 공격력 {GameManager.Instance.Archer.Data.Damage}");
    }
}