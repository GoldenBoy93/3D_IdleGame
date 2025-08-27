using System;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀을 관리하는 매니저 클래스
public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // 풀링할 프리팹들
    // 프리팹 인덱스별 오브젝트 큐
    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();

    public static ObjectPoolManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        // 각 프리팹 인덱스에 대한 큐 초기화
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new Queue<GameObject>();
        }
    }

    // 오브젝트 요청 시 호출 (없으면 새로 생성, 있으면 재사용)
    public GameObject GetObject(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        // 지정된 인덱스에 대한 풀이 존재하지 않는 경우
        if (!pools.ContainsKey(prefabIndex))
        {
            Debug.LogError($"프리팹 인덱스 {prefabIndex}에 대한 풀이 존재하지 않습니다.");
            return null;
        }

        GameObject obj;
        // 풀에 남아있는 오브젝트가 있다면 꺼내기
        if (pools[prefabIndex].Count > 0)
        {
            obj = pools[prefabIndex].Dequeue();
        }
        else
        {
            // 없다면 새로 생성 후 반환처리를 위한 초기화
            obj = Instantiate(prefabs[prefabIndex]);
            obj.GetComponent<IPoolable>()?.Initialize(o => ReturnObject(prefabIndex, o));
        }

        // 위치, 회전 설정 및 활성화
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn(); // 풀에서 꺼낼 때 처리
        return obj;
    }

    // 오브젝트를 풀로 반환하는 함수
    public void ReturnObject(int prefabIndex, GameObject obj)
    {
        if (!pools.ContainsKey(prefabIndex))
        {
            Destroy(obj); // 풀에 해당 인덱스가 없으면 삭제
            return;
        }

        obj.SetActive(false); // 비활성화 후 큐에 다시 넣음
        pools[prefabIndex].Enqueue(obj);
    }
}