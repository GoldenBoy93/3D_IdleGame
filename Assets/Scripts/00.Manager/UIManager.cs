using UnityEngine;
using TMPro; // TextMeshPro를 사용하는 경우

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    // 싱글톤
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에 Manager가 없으면 에러를 발생시켜 문제를 알림
                Debug.LogError("UIManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // UI 요소들을 직접 참조할 변수들
    public GameObject mainUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI stageNumber;
    public TextMeshProUGUI pointNumber; // 포인트를 Text로 치환
    private int pointValue = 0; // 누적 포인트 숫자 값
    public GameObject shopUI; // 상점 UI를 포함하는 최상위 게임 오브젝트

    private void Awake()
    {
        _instance = this;
    }

    // 메인 UI 활성화
    public void ShowManiUI()
    {
        if (mainUI != null) mainUI.SetActive(true);
    }

    // 게임오버 UI 활성화
    public void ShowGameOverUI()
    {
        if (mainUI != null) mainUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    public void ShowShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
    }

    public void HideShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
    }

    // GameManager의 OnStartWave 이벤트에 OnGameManagerStartWave 메서드를 등록
    private void OnEnable()
    {
        GameManager.OnStartWave += OnGameManagerStartWave;
    }
    // 오브젝트가 비활성화될 때 이벤트 구독 해제
    private void OnDisable()
    {
        GameManager.OnStartWave -= OnGameManagerStartWave;
    }

    // 이벤트가 발생할 때 호출되는 메서드
    private void OnGameManagerStartWave(int waveCount)
    {
        // 메인UI 활성화
        ShowManiUI();
        // 스테이지 UI 업데이트
        stageNumber.text = $"{waveCount}";
    }

    // 외부에서 포인트를 추가할 때 호출 할 함수
    public void UpdatePointText(int point)
    {
        // 호출한 곳에서 받아온 매개변수 값을 현재의 포인트에 더한다.
        pointValue += point;

        // 포인트 UI 업데이트
        pointNumber.text = $"{pointValue}";
    }
}