using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // PlayerInput의 Behavior : Invoke C Sharp Events 설정으로 생성된 그것!
    public PlayerInputs playerInput { get; private set; }
    // 그 중 가져올 Action Map을 저장할 변수
    public PlayerInputs.PlayerActions playerActions { get; private set; }

    private void Awake()
    {
        playerInput = new PlayerInputs();
        // playerInput.Player 는 플레이어 인풋 시스템 가장 좌측의 'Player' Action Maps 항목에 해당
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
}