using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherManager : MonoBehaviour
{
    private static ArcherManager _instance;

    private Coroutine waveRoutine;

    [SerializeField]
    private List<GameObject> ArcherPrefabs; // 생성할 적 프리팹 리스트

    [SerializeField]
    private List<Rect> spawnAreas; // 적을 생성할 영역 리스트

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 기즈모 색상

    private List<Archer> activeEnemies = new List<Archer>(); // 현재 생성 된 적들을 저장 할 'Archer' 타입의 List 선언

    private bool ArcherSpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager; // 게임매니저를 지칭 할 변수

    ObjectPoolManager objectPoolManager;  // 오브젝트풀매니저를 지칭 할 변수


    // ArcherManager 싱글톤
    public static ArcherManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        // 게임매니저를 변수에 저장
        gameManager = GameManager.Instance;

        // ObjectPoolManager 싱글톤 인스턴스를 가져옴
        objectPoolManager = ObjectPoolManager.Instance;
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
        StartWave(waveCount);
    }

    // 기존 StartWave 메서드
    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            // GameManager에게 웨이브가 끝났음을 알림
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndOfWave();
            }
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
        ArcherSpawnComplete = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomArcher();
        }

        ArcherSpawnComplete = true;
    }

    private void SpawnRandomArcher()
    {
        if (ArcherPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Archer Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        // 랜덤한 적 프리팹 선택
        GameObject randomPrefab = ArcherPrefabs[Random.Range(0, ArcherPrefabs.Count)];

        // 선택된 프리팹의 인덱스를 가져옴
        int prefabIndex = ArcherPrefabs.IndexOf(randomPrefab);

        // 랜덤한 영역 선택
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect 영역 내부의 랜덤한 3D 위치 계산
        Vector3 randomPosition = new Vector3(Random.Range(randomArea.xMin, randomArea.xMax), 0f, Random.Range(randomArea.yMin, randomArea.yMax));

        // 오브젝트 풀에서 인덱스를 사용하여 오브젝트를 가져옴
        GameObject spawnedArcher = objectPoolManager.GetObject(prefabIndex, randomPosition, Quaternion.identity);

        Archer Archer = spawnedArcher.GetComponent<Archer>();

        // 오브젝트가 올바르게 생성되었을 경우에만 리스트에 추가
        if (Archer != null)
        {
            activeEnemies.Add(Archer);
        }
        else
        {
            Debug.LogError("오브젝트 풀에서 가져온 오브젝트에 Archer 컴포넌트가 없습니다!");
        }

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

    // Archer 스크립트의 OnDie 함수에서 호출 (해당 에너미 객체가 죽었을때)
    public void RemoveArcherOnDeath(Archer Archer)
    {
        // activeEnemies라는 List에서 매개변수로 받아온 Archer 객체를 제거
        activeEnemies.Remove(Archer);

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");

        // 스폰이 종료 되었고, List의 Archer가 0일때 (모두 사망했을때)
        if (ArcherSpawnComplete && activeEnemies.Count == 0)
            gameManager.EndOfWave();
    }
}
