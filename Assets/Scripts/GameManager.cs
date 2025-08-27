using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private int currentWaveIndex = 0;

    // 다음 웨이브를 시작해야 할 때 호출되는 이벤트
    public delegate void StartWaveAction(int waveCount);
    public static event StartWaveAction OnStartWave;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에 GameManager가 없으면 에러를 발생시켜 문제를 알림
                Debug.LogError("GameManager is not found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 싱글톤 패턴 초기화 (중복 로직 제거)
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
        // 게임 시작 시 첫 웨이브 시작
        StartNextWave();
    }

    // 다음 웨이브를 시작하는 핵심 로직
    void StartNextWave()
    {
        currentWaveIndex += 1;
        int enemyCount = 1 + currentWaveIndex / 2;

        // 이벤트를 호출하여 다음 웨이브 시작을 알림
        // EnemyManager는 이 이벤트를 구독하여 웨이브를 시작하게 됨
        if (OnStartWave != null)
        {
            OnStartWave(enemyCount);
            Debug.Log($"Next : {enemyCount}");
        }
    }

    // EnemyManager로부터 호출될 메서드
    public void EndOfWave()
    {
        // 다음 웨이브 시작
        StartNextWave();
    }
}