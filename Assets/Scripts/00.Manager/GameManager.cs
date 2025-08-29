using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� Manager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("GameManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // Archer ��ũ��Ʈ Awake���� �ν��Ͻ� �޾ƿ�
    private Archer _archer;

    // Archer ��ũ��Ʈ���� �޾ƿ� �ν��Ͻ��� �ٸ� ������ ����� �� �ֵ��� ������Ƽ ���
    public Archer Archer
    {
        get { return _archer; }
        set { _archer = value; }
    }

    [SerializeField] private int currentWaveIndex = 0;

    // ���� ���̺긦 �����ؾ� �� �� ȣ��Ǵ� �̺�Ʈ
    public delegate void StartWaveAction(int waveCount);
    public static event StartWaveAction OnStartWave;

    private void Awake()
    {
        _instance = this;

        // �̱��� ���� �ʱ�ȭ (�ߺ� ���� ����)
        //if (_instance == null)
        //{
        //    _instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    if (_instance != this)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
    }

    private void Start()
    {
        // ���� ���� �� ù ���̺� ����
        StartNextWave();
    }

    // ���� ���̺긦 �����ϴ� �ٽ� ����
    void StartNextWave()
    {
        // ������ �� ����
        currentWaveIndex += 1;
        // ���� �� ������ ������ ����Ͽ� ������ ���� ��Ʈ��
        int enemyCount = 1 + currentWaveIndex / 2;

        // �̺�Ʈ�� ȣ���Ͽ� ���� ���̺� ������ �˸�
        // ������ : EnemyManager, UIManager
        if (OnStartWave != null)
        {
            OnStartWave(enemyCount);
        }
    }

    // EnemyManager�κ��� ȣ��� �޼���
    public void EndOfWave()
    {
        // ���� ���̺� ����
        StartNextWave();
    }
}