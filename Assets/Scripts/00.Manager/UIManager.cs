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
    public TextMeshProUGUI waveText;
    //public TextMeshProUGUI PointText;

    GameManager gameManager; // 게임매니저를 지칭 할 변수

    private void Awake()
    {
        _instance = this;

        // 게임매니저를 변수에 저장
        gameManager = GameManager.Instance;
    }

    public void UpdateWaveText(int waveNumber)
    {
        if (waveText != null)
        {
            waveText.text = "Wave " + waveNumber;
        }
    }

    //public void UpdatePointText(int score)
    //{
    //    if (scoreText != null)
    //    {
    //        scoreText.text = "Score: " + score;
    //    }
    //}

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

    private void OnEnable()
    {
        // GameManager의 OnStartWave 이벤트에 OnGameManagerStartWave 메서드를 등록
        GameManager.OnStartWave += OnGameManagerStartWave;
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 이벤트 구독 해제
        GameManager.OnStartWave -= OnGameManagerStartWave;
    }

    // 이벤트가 발생할 때 호출되는 메서드
    private void OnGameManagerStartWave(int waveCount)
    {
        // 웨이브 UI 업데이트
    }
}