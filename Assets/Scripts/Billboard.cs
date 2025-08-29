using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // 메인 카메라의 Transform을 찾습니다.
        mainCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Canvas가 항상 카메라를 바라보도록 회전시킵니다.
        // LookAt() 함수는 지정된 대상(카메라)을 바라보게 합니다.
        // Vector3.up은 오브젝트의 위쪽 방향이 월드의 위쪽과 일치하도록 합니다.
        transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                         mainCameraTransform.rotation * Vector3.up);
    }
}