using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    // �̱��� ���� (���� �ڵ� ����)
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
        // ���� ���� �� ��Ʈ�� ������ �ڵ� ����
        StartGameSequence();
    }

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _introCam;
    [SerializeField] public CinemachineVirtualCamera _mainCam;

    // �÷��̾� �Է� ��ũ��Ʈ ����
    [SerializeField] private PlayerController playerController;

    public void StartGameSequence()
    {
        // ������ ������ �� �÷��̾� �Է��� ��Ȱ��ȭ
        if (playerController != null)
        {
            playerController.LockOnInput(1); // 1�� �����Ͽ� �Է� ��Ȱ��ȭ
        }

        StartCoroutine(Sequence());
    }

    public IEnumerator Sequence()
    {
        // ��Ʈ�� ī�޶� ���� ī�޶󺸴� ���� �켱������ �����ϵ��� ����
        if (_introCam != null)
        {
            _introCam.Priority = 20;
        }
        if (_mainCam != null)
        {
            _mainCam.Priority = 10;
        }

        // ���� �ð���ŭ ���
        yield return new WaitForSecondsRealtime(5f);

        // ������ ������ ��Ʈ�� ī�޶��� �켱������ ���� ���� ī�޶�� ��ȯ
        if (_introCam != null)
        {
            _introCam.Priority = 5; // ���� ī�޶�(10)���� ���� �켱����
        }

        // ���� ī�޶�� ��ȯ �� �÷��̾� �Է��� Ȱ��ȭ
        if (playerController != null)
        {
            playerController.LockOnInput(0); // 0�� �����Ͽ� �Է� Ȱ��ȭ
        }
    }
}