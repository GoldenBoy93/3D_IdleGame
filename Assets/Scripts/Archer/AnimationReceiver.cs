using UnityEngine;

public class AnimationReceiver : MonoBehaviour
{
    private Archer archer;

    private void Awake()
    {
        archer = transform.parent.GetComponent<Archer>();

        if (archer == null)
        {
            Debug.LogError("부모 오브젝트에 Archer 스크립트가 없습니다!");
        }
    }

    // 애니메이션 이벤트에서 호출할 함수
    public void FireArrowEvent()
    {
        if (archer != null)
        {
            // Archer 스크립트의 FireArrowEvent() 함수를 호출
            archer.FireArrowEvent();
        }
    }
}