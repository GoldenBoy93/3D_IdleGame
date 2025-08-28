using System;
using UnityEngine;

// �̰� ���̸� ����ȭ �Ǿ� 'Archer' ��ũ��Ʈó�� [field : SerializeField]�� �ٿ���
// ����Ƽ���� ���������� 'Archer' �ν�����â�� ���� �� �� ����.
[Serializable]
public class ArcherAnimationData
{
    // AnimationController�� Parameter �̸����� �״�� ������
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string attackParameterName = "Attack";

    // Hash ������ ���ϱ� ���� ��ȯ�� ���� ������ ������
    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    // Archer�� Awake���� ȣ�� ����
    public void Initialize()
    {
        // Hash�� ��ȯ�Ͽ� ������ ����
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
    }
}