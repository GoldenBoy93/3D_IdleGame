using UnityEngine;
using TMPro; // TextMeshPro�� ����ϴ� ���

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    // �̱���
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� Manager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("UIManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // UI ��ҵ��� ���� ������ ������
    public GameObject mainUI;
    public GameObject gameOverUI;
    public TextMeshProUGUI waveText;
    //public TextMeshProUGUI PointText;

    GameManager gameManager; // ���ӸŴ����� ��Ī �� ����

    private void Awake()
    {
        _instance = this;

        // ���ӸŴ����� ������ ����
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

    // ���� UI Ȱ��ȭ
    public void ShowManiUI()
    {
        if (mainUI != null) mainUI.SetActive(true);
    }

    // ���ӿ��� UI Ȱ��ȭ
    public void ShowGameOverUI()
    {
        if (mainUI != null) mainUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(true);
    }

    private void OnEnable()
    {
        // GameManager�� OnStartWave �̺�Ʈ�� OnGameManagerStartWave �޼��带 ���
        GameManager.OnStartWave += OnGameManagerStartWave;
    }

    private void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� �̺�Ʈ ���� ����
        GameManager.OnStartWave -= OnGameManagerStartWave;
    }

    // �̺�Ʈ�� �߻��� �� ȣ��Ǵ� �޼���
    private void OnGameManagerStartWave(int waveCount)
    {
        // ���̺� UI ������Ʈ
    }
}