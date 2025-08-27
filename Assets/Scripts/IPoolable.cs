using UnityEngine;
using System;

// ������Ʈ Ǯ���� ���� �������̽�
public interface IPoolable
{
    // ������Ʈ�� Ǯ�� ���� ������ �� ȣ���
    // ��ȯ ó���� ���� �ݹ�(Action)�� ���޹���
    void Initialize(Action<GameObject> returnAction);

    // ������Ʈ�� Ǯ���� ������ Ȱ��ȭ�� �� ȣ���
    void OnSpawn();

    // ������Ʈ�� ��� ����Ǿ� ��Ȱ��ȭ�� �� ȣ���
    void OnDespawn();
}