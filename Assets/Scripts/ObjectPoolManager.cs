using System;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ Ǯ�� �����ϴ� �Ŵ��� Ŭ����
public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // Ǯ���� �����յ�
    // ������ �ε����� ������Ʈ ť
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        // �� ������ �ε����� ���� ť �ʱ�ȭ
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    // ������Ʈ ��û �� ȣ�� (������ ���� ����, ������ ����)
    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        // ������ �ε����� ���� Ǯ�� �������� �ʴ� ���
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.LogError($"������ �ε��� {prefabIndex}�� ���� Ǯ�� �������� �ʽ��ϴ�.");
            return null;
        }

        GameObject obj;
        // Ǯ�� �����ִ� ������Ʈ�� �ִٸ� ������
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else
        {
            // ���ٸ� ���� ���� �� ��ȯó���� ���� �ʱ�ȭ
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }

        // ��ġ, ȸ�� ���� �� Ȱ��ȭ
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn(); // Ǯ���� ���� �� ó��
        return obj;
    }

    // ������Ʈ�� Ǯ�� ��ȯ�ϴ� �Լ�
    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(obj); // Ǯ�� �ش� �ε����� ������ ����
            return;
        }

        obj.SetActive(false); // ��Ȱ��ȭ �� ť�� �ٽ� ����
        pools[prefabIndex].Enqueue(obj);
    }
}