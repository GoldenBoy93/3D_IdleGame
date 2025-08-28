using UnityEngine;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    // PlayerInput�� Behavior : Invoke C Sharp Events �������� ������ �װ�!
    public PlayerInputs playerInput { get; private set; }
    // �� �� ������ Action Map�� ������ ����
    public PlayerInputs.PlayerActions playerActions { get; private set; }

    private void Awake()
    {
        playerInput = new PlayerInputs();
        // playerInput.Player �� �÷��̾� ��ǲ �ý��� ���� ������ 'Player' Action Maps �׸� �ش�
        playerActions = playerInput.Player;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void LockOnInput(int value)
    {
        if (value == 1)
        {
            playerInput.Player.Disable();
        }
        else
        {
            playerInput.Player.Enable();
        }
    }
}