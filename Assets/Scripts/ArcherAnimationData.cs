using System;
using UnityEngine;

// 이걸 붙이면 직렬화 되어 'Player' 스크립트처럼 [field : SerializeField]를 붙여서
// 유니티에서 내부적으로 'Player' 인스펙터창에 갖다 쓸 수 있음.
[Serializable]
public class ArcherAnimationData
{
    // AnimationController의 Parameter 이름들을 그대로 가져옴
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string attackParameterName = "Attack";

    // Hash 값으로 비교하기 위해 변환한 값을 저장할 변수들
    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    // Player의 Awake에서 호출 예정
    public void Initialize()
    {
        // Hash로 변환하여 변수에 저장
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
    }
}