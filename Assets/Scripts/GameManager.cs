using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private int currentWaveIndex = 0;

    // ���� ���̺긦 �����ؾ� �� �� ȣ��Ǵ� �̺�Ʈ
    public delegate void StartWaveAction(int waveCount);
    public static event StartWaveAction OnStartWave;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� GameManager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("GameManager is not found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // �̱��� ���� �ʱ�ȭ (�ߺ� ���� ����)
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        // ���� ���� �� ù ���̺� ����
        StartNextWave();
    }

    // ���� ���̺긦 �����ϴ� �ٽ� ����
    void StartNextWave()
    {
        currentWaveIndex += 1;
        int enemyCount = 1 + currentWaveIndex / 2;

        // �̺�Ʈ�� ȣ���Ͽ� ���� ���̺� ������ �˸�
        // EnemyManager�� �� �̺�Ʈ�� �����Ͽ� ���̺긦 �����ϰ� ��
        if (OnStartWave != null)
        {
            OnStartWave(enemyCount);
            Debug.Log($"Next : {enemyCount}");
        }
    }

    // EnemyManager�κ��� ȣ��� �޼���
    public void EndOfWave()
    {
        // ���� ���̺� ����
        StartNextWave();
    }
}