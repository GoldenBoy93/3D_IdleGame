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
        // ���� ���� ó��
        // ��: �÷��̾��� ���� Ȯ�� ��, ���� �����ϸ� itemData.ApplyEffect() ȣ��
        if (itemData != null)
        {
            itemData.ApplyEffect();
        }
    }
}