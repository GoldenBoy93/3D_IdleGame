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
                // 씬에 Manager가 없으면 에러를 발생시켜 문제를 알림
                Debug.LogError("GameManager is not found in the scene.");
            }
            return _instance;
        }
    }

    // Archer 스크립트 Awake에서 인스턴스 받아옴
    private Archer _archer;

    // Archer 스크립트에서 받아온 인스턴스를 다른 곳에서 사용할 수 있도록 프로퍼티 사용
    public Archer Archer
    {
        get { return _archer; }
        set { _archer = value; }
    }

    [SerializeField] private int currentWaveIndex = 0;

    // 다음 웨이브를 시작해야 할 때 호출되는 이벤트
    public delegate void StartWaveAction(int waveCount);
    public static event StartWaveAction OnStartWave;

    private void Awake()
    {
        _instance = this;

        // 싱글톤 패턴 초기화 (중복 로직 제거)
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
        // 게임 시작 시 첫 웨이브 시작
        StartNextWave();
    }

    // 다음 웨이브를 시작하는 핵심 로직
    void StartNextWave()
    {
        // 생성할 적 숫자
        currentWaveIndex += 1;
        // 몫을 뺀 나머지 공식을 사용하여 적생성 숫자 컨트롤
        int enemyCount = 1 + currentWaveIndex / 2;

        // 이벤트를 호출하여 다음 웨이브 시작을 알림
        // 구독자 : EnemyManager, UIManager
        if (OnStartWave != null)
        {
            OnStartWave(enemyCount);
        }
    }

    // EnemyManager로부터 호출될 메서드
    public void EndOfWave()
    {
        // 다음 웨이브 시작
        StartNextWave();
    }
}