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
    private List<GameObject> ArcherPrefabs; // ������ �� ������ ����Ʈ

    [SerializeField]
    private List<Rect> spawnAreas; // ���� ������ ���� ����Ʈ

    [SerializeField]
    private Color gizmoColor = new Color(1, 0, 0, 0.3f); // ����� ����

    private List<Archer> activeEnemies = new List<Archer>(); // ���� ���� �� ������ ���� �� 'Archer' Ÿ���� List ����

    private bool ArcherSpawnComplete;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

    GameManager gameManager; // ���ӸŴ����� ��Ī �� ����

    ObjectPoolManager objectPoolManager;  // ������ƮǮ�Ŵ����� ��Ī �� ����


    // ArcherManager �̱���
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
            Debug.LogWarning("Archer Prefabs �Ǵ� Spawn Areas�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ������ �� ������ ����
        GameObject randomPrefab = ArcherPrefabs[Random.Range(0, ArcherPrefabs.Count)];

        // ���õ� �������� �ε����� ������
        int prefabIndex = ArcherPrefabs.IndexOf(randomPrefab);

        // ������ ���� ����
        Rect randomArea = spawnAreas[Random.Range(0, spawnAreas.Count)];

        // Rect ���� ������ ������ 3D ��ġ ���
        Vector3 randomPosition = new Vector3(Random.Range(randomArea.xMin, randomArea.xMax), 0f, Random.Range(randomArea.yMin, randomArea.yMax));

        // ������Ʈ Ǯ���� �ε����� ����Ͽ� ������Ʈ�� ������
        GameObject spawnedArcher = objectPoolManager.GetObject(prefabIndex, randomPosition, Quaternion.identity);

        Archer Archer = spawnedArcher.GetComponent<Archer>();

        // ������Ʈ�� �ùٸ��� �����Ǿ��� ��쿡�� ����Ʈ�� �߰�
        if (Archer != null)
        {
            activeEnemies.Add(Archer);
        }
        else
        {
            Debug.LogError("������Ʈ Ǯ���� ������ ������Ʈ�� Archer ������Ʈ�� �����ϴ�!");
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

    // Archer ��ũ��Ʈ�� OnDie �Լ����� ȣ�� (�ش� ���ʹ� ��ü�� �׾�����)
    public void RemoveArcherOnDeath(Archer Archer)
    {
        // activeEnemies��� List���� �Ű������� �޾ƿ� Archer ��ü�� ����
        activeEnemies.Remove(Archer);

        Debug.Log($"activeEnemies List : {activeEnemies.Count}");

        // ������ ���� �Ǿ���, List�� Archer�� 0�϶� (��� ���������)
        if (ArcherSpawnComplete && activeEnemies.Count == 0)
            gameManager.EndOfWave();
    }
}
