using UnityEngine;

// 유니티 에디터에서 쉽게 생성할 수 있도록 메뉴 아이템 추가
[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop/Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemPrice;
    public Sprite itemIcon;

    // 아이템 효과를 적용하는 추상화된 함수
    public virtual void ApplyEffect()
    {
        Debug.Log("이 아이템은 효과가 정의되지 않았습니다.");
    }
}