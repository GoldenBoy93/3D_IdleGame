using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    private Archer archer;

    // Start 또는 Awake에서 부모 오브젝트의 Archer 스크립트 인스턴스를 가져옴
    private void Awake()
    {
        // GetParentsComponent<T>는 자식 오브젝트에서 부모 오브젝트의 컴포넌트를 찾는 확장 메서드입니다.
        // 또는 transform.parent.GetComponent<Archer>()를 사용할 수 있습니다.
        archer = transform.parent.GetComponent<Archer>();

        if (archer == null)
        {
            Debug.LogError("부모 오브젝트에 Archer 스크립트가 없습니다!");
        }
    }

    // 애니메이션 이벤트에서 호출할 함수
    public void FireArrowEvent()
    {
        // 부모 오브젝트의 Archer 스크립트에 있는 함수를 호출
        if (archer != null)
        {
            // Archer 스크립트의 FireArrowEvent() 함수를 호출합니다.
            // Archer 스크립트의 FireArrowEvent() 함수는 public이어야 합니다.
            archer.FireArrowEvent();
        }
    }
}