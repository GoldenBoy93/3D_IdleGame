using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;

    public static bool isFirstLoading = true;


    public static GameManager Instance
    {
        get
        {
            // 할당되지 않았을 때, 외부에서 GameManager.Instance 로 접근하는 경우
            // 게임 오브젝트를 만들어주고 GameManager 스크립트를 AddComponent로 붙여준다.
            if (_instance == null)
            {
                // 게임오브젝트가 없어도 시작시 없는걸 확인후 매니저를 게임오브젝트로 생성해줌
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Awake가 호출 될 때라면 이미 매니저 오브젝트는 생성되어 있는 것이고, '_instance'에 자신을 할당
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 오브젝트가 존재하는 경우 '자신'을 파괴해서 중복방지
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        enemyManager = EnemyManager.Instance;
    }

    private void Start()
    {
        StartGame();

        //if (!isFirstLoading)
        //{
        //    StartGame();
        //}
        //else
        //{
        //    isFirstLoading = false;
        //}
    }

    public void StartGame()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex += 1;
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
    }
}
