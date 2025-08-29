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
                // 씬에 Manager가 없으면 에러를 발생시켜 문제를 알림
                Debug.LogError("ShopManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // 유니티 인스펙터에 ShopItemData 리스트를 할당
    public List<ShopItemSO> availableItems;

    // 상점 슬롯 UI 프리팹
    public GameObject slotUIPrefab;

    // 슬롯을 배치할 부모 UI Transform
    public Transform slotParent;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // 모든 아이템 데이터를 순회하며 UI 슬롯을 생성하고 초기화
        DisplayShopItems();
    }

    private void DisplayShopItems()
    {
        // 아이템 리스트를 순회하며 슬롯 UI 생성
        foreach (var item in availableItems)
        {
            GameObject slotGO = Instantiate(slotUIPrefab, slotParent);
            ShopSlotUI slotUI = slotGO.GetComponent<ShopSlotUI>();
            if (slotUI != null)
            {
                // 스크립터블 오브젝트 데이터를 슬롯에 할당
                slotUI.SetupSlot(item);
            }
        }
    }
}
