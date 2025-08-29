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
    public TextMeshProUGUI stageNumber;
    public TextMeshProUGUI pointNumber; // ����Ʈ�� Text�� ġȯ
    private int pointValue = 0; // ���� ����Ʈ ���� ��
    public GameObject shopUI; // ���� UI�� �����ϴ� �ֻ��� ���� ������Ʈ

    private void Awake()
    {
        _instance = this;
    }

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

    // GameManager�� OnStartWave �̺�Ʈ�� OnGameManagerStartWave �޼��带 ���
    private void OnEnable()
    {
        GameManager.OnStartWave += OnGameManagerStartWave;
    }
    // ������Ʈ�� ��Ȱ��ȭ�� �� �̺�Ʈ ���� ����
    private void OnDisable()
    {
        GameManager.OnStartWave -= OnGameManagerStartWave;
    }

    // �̺�Ʈ�� �߻��� �� ȣ��Ǵ� �޼���
    private void OnGameManagerStartWave(int waveCount)
    {
        // ����UI Ȱ��ȭ
        ShowManiUI();
        // �������� UI ������Ʈ
        stageNumber.text = $"{waveCount}";
    }

    // �ܺο��� ����Ʈ�� �߰��� �� ȣ�� �� �Լ�
    public void UpdatePointText(int point)
    {
        // ȣ���� ������ �޾ƿ� �Ű����� ���� ������ ����Ʈ�� ���Ѵ�.
        pointValue += point;

        // ����Ʈ UI ������Ʈ
        pointNumber.text = $"{pointValue}";
    }
}