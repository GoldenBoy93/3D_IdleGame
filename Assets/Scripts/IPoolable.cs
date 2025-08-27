using UnityEngine;
using System;

// 오브젝트 풀링을 위한 인터페이스
public interface IPoolable
{
    // 오브젝트가 풀에 의해 생성될 때 호출됨
    // 반환 처리를 위한 콜백(Action)을 전달받음
    void Initialize(Action<GameObject> returnAction);

    // 오브젝트가 풀에서 꺼내져 활성화될 때 호출됨
    void OnSpawn();

    // 오브젝트가 사용 종료되어 비활성화될 때 호출됨
    void OnDespawn();
}