using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static ShopManager _instance;

    public static ShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� Manager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("ShopManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // ����Ƽ �ν����Ϳ� ShopItemData ����Ʈ�� �Ҵ�
    public List<ShopItemSO> availableItems;

    // ���� ���� UI ������
    public GameObject slotUIPrefab;

    // ������ ��ġ�� �θ� UI Transform
    public Transform slotParent;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // ��� ������ �����͸� ��ȸ�ϸ� UI ������ �����ϰ� �ʱ�ȭ
        DisplayShopItems();
    }

    private void DisplayShopItems()
    {
        // ������ ����Ʈ�� ��ȸ�ϸ� ���� UI ����
        foreach (var item in availableItems)
        {
            GameObject slotGO = Instantiate(slotUIPrefab, slotParent);
            ShopSlotUI slotUI = slotGO.GetComponent<ShopSlotUI>();
            if (slotUI != null)
            {
                // ��ũ���ͺ� ������Ʈ �����͸� ���Կ� �Ҵ�
                slotUI.SetupSlot(item);
            }
        }
    }
}
