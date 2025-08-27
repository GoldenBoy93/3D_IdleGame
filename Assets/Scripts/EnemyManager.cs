using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    private Coroutine waveRoutine;

    [SerializeField]
    private List<GameObject> enemyPrefabs; // 생성할 적 프리팹 리스트

    [SerializeField]
    private List<Rect> spawnAreas; // 적을 생성할 영역 리스트

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상

    private List<Enemy> activeEnemies = new List<Enemy>(); // 현재 생성 된 적들을 저장 할 'Enemy' 타입의 List 선언

    private bool enemySpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager; // 게임매니저를 지칭 할 변수

    public static EnemyManager Instance
    {
        get
        {
            // 할당되지 않았을 때, 외부에서 Manager.Instance 로 접근하는 경우
            // 게임 오브젝트를 만들어주고 Manager 스크립트를 AddComponent로 붙여준다.
            if (_instance == null)
            {
                // 게임오브젝트가 없어도 시작시 없는걸 확인후 매니저를 게임오브젝트로 생성해줌
                _instance = new GameObject("EnemyManager").AddComponent<EnemyManager>();
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

        // 게임매니저를 변수에 저장
        gameManager = GameManager.Instance;
    }


    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            gameManager.EndOfWave();
            return;
        }

        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplete = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomEnemy();
        }

        enemySpawnComplete = true;
    }

    private void SpawnRandomEnemy()
    {
        if (enemyPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Enemy Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        // 랜덤한 적 프리팹 선택
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 랜덤한 영역 선택
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect 영역 내부의 랜덤한 3D 위치 계산
        // y축은 지면의 높이 (예: 0)로 고정
        Vector3 randomPosition = new Vector3(Random.Range(randomArea.xMin, randomArea.xMax), 0f, Random.Range(randomArea.yMin, randomArea.yMax));

        // 적 생성 및 리스트에 추가
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity);
        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

        activeEnemies.Add(enemy);

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");
    }

    // 기즈모를 그려 영역을 시각화 (이 컴포넌트가 붙은 객체가 선택된 경우에만 표시)
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            // Vector3의 y 값을 0으로 설정하여 XZ 평면에 기즈모를 그리기
            Vector3 center = new Vector3(area.x + area.width / 2, 0f, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, 0.1f, area.height); // 높이를 아주 얇게 설정 (시각화를 위해)
            Gizmos.DrawCube(center, size);
        }
    }

    // Enemy 스크립트의 OnDie 함수에서 호출 (해당 에너미 객체가 죽었을때)
    public void RemoveEnemyOnDeath(Enemy enemy)
    {
        // activeEnemies라는 List에서 매개변수로 받아온 enemy 객체를 제거
        activeEnemies.Remove(enemy);

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");

        // 스폰이 종료 되었고, List의 enemy가 0일때 (모두 사망했을때)
        if (enemySpawnComplete && activeEnemies.Count == 0)
            gameManager.EndOfWave();
    }
}
