using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;

    public static EnemyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� Manager�� ������ ������ �߻����� ������ �˸�
                Debug.LogError("EnemyManager is not found in the scene.");
            }
            return _instance;
        }
    }

    private Coroutine waveRoutine;

    [SerializeField]
    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    [SerializeField]
    private List<Rect> spawnAreas; // ���� ������ ���� ����Ʈ

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // ����� ����

    private List<Enemy> activeEnemies = new List<Enemy>(); // ���� ���� �� ������ ���� �� 'Enemy' Ÿ���� List ����

    private bool enemySpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager; // ���ӸŴ����� ��Ī �� ����

    ObjectPoolManager objectPoolManager;  // ������ƮǮ�Ŵ����� ��Ī �� ����

    private void Awake()
    {
        _instance = this;

        // ���ӸŴ����� ������ ����
        gameManager = GameManager.Instance;

        // ObjectPoolManager �̱��� �ν��Ͻ��� ������
        objectPoolManager = ObjectPoolManager.Instance;
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
        StartWave(waveCount);
    }

    // ���� StartWave �޼���
    public void StartWave(int waveCount)
    {
        if (waveCount <= 0)
        {
            // GameManager���� ���̺갡 �������� �˸�
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
            Debug.LogWarning("Enemy Prefabs �Ǵ� Spawn Areas�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ������ �� ������ ����
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // ���õ� �������� �ε����� ������
        int prefabIndex = enemyPrefabs.IndexOf(randomPrefab);

        // ������ ���� ����
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect ���� ������ ������ 3D ��ġ ���
        Vector3 randomPosition = new Vector3(Random.Range(randomArea.xMin, randomArea.xMax), 0f, Random.Range(randomArea.yMin, randomArea.yMax));

        // ������Ʈ Ǯ���� �ε����� ����Ͽ� ������Ʈ�� ������
        GameObject spawnedEnemy = objectPoolManager.GetObject(prefabIndex, randomPosition, Quaternion.identity);

        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

        // ������Ʈ�� �ùٸ��� �����Ǿ��� ��쿡�� ����Ʈ�� �߰�
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
        }
        else
        {
            Debug.LogError("������Ʈ Ǯ���� ������ ������Ʈ�� Enemy ������Ʈ�� �����ϴ�!");
        }

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");
    }

    // ����� �׷� ������ �ð�ȭ (�� ������Ʈ�� ���� ��ü�� ���õ� ��쿡�� ǥ��)
    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        Gizmos.color = gizmoColor;
        foreach (var area in spawnAreas)
        {
            // Vector3�� y ���� 0���� �����Ͽ� XZ ��鿡 ����� �׸���
            Vector3 center = new Vector3(area.x + area.width / 2, 0f, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, 0.1f, area.height); // ���̸� ���� ��� ���� (�ð�ȭ�� ����)
            Gizmos.DrawCube(center, size);
        }
    }

    // Enemy ��ũ��Ʈ�� OnDie �Լ����� ȣ�� (�ش� ���ʹ� ��ü�� �׾�����)
    public void RemoveEnemyOnDeath(Enemy enemy)
    {
        // activeEnemies��� List���� �Ű������� �޾ƿ� enemy ��ü�� ����
        activeEnemies.Remove(enemy);

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");

        // ������ ���� �Ǿ���, List�� enemy�� 0�϶� (��� ���������)
        if (enemySpawnComplete && activeEnemies.Count == 0)
            gameManager.EndOfWave();
    }
}
