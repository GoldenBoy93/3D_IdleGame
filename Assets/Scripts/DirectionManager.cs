using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    // 싱글톤 패턴 (기존 코드 유지)
    private static DirectionManager _instance;
    public static DirectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DirectionManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("DirectionManager");
                    _instance = go.AddComponent<DirectionManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 게임 시작 시 인트로 시퀀스 자동 시작
        StartGameSequence();
    }

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _introCam;
    [SerializeField] public CinemachineVirtualCamera _mainCam;

    // 플레이어 입력 스크립트 참조
    [SerializeField] private PlayerController playerController;

    public void StartGameSequence()
    {
        // 연출을 시작할 때 플레이어 입력을 비활성화
        if (playerController != null)
        {
            playerController.LockOnInput(1); // 1을 전달하여 입력 비활성화
        }

        StartCoroutine(Sequence());
    }

    public IEnumerator Sequence()
    {
        // 인트로 카메라가 메인 카메라보다 높은 우선순위를 유지하도록 설정
        if (_introCam != null)
        {
            _introCam.Priority = 20;
        }
        if (_mainCam != null)
        {
            _mainCam.Priority = 10;
        }

        // 연출 시간만큼 대기
        yield return new WaitForSecondsRealtime(5f);

        // 연출이 끝나면 인트로 카메라의 우선순위를 낮춰 메인 카메라로 전환
        if (_introCam != null)
        {
            _introCam.Priority = 5; // 메인 카메라(10)보다 낮은 우선순위
        }

        // 메인 카메라로 전환 후 플레이어 입력을 활성화
        if (playerController != null)
        {
            playerController.LockOnInput(0); // 0을 전달하여 입력 활성화
        }
    }
}