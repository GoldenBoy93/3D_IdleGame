using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public ShopItemSO itemData;
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    public void SetupSlot(ShopItemSO data)
    {
        itemData = data;
        if (itemData != null)
        {
            Debug.Log(itemData.itemIcon);
            itemIconImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemPriceText.text = itemData.itemPrice.ToString();
            buyButton.onClick.AddListener(OnBuyButtonClick);
        }
    }

    private void OnBuyButtonClick()
    {
        // 구매 로직 처리
        // 예: 플레이어의 코인 확인 후, 구매 가능하면 itemData.ApplyEffect() 호출
        if (itemData != null)
        {
            itemData.ApplyEffect();
        }
    }
}