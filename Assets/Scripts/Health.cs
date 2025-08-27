using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    public event Action OnDie;

    public bool IsDie = false;

    private void Start()
    {
        // 플레이어처럼 오브젝트 풀링을 사용하지 않는 경우를 위해 체력 초기화
        health = maxHealth;
        IsDie = false;
    }

    // 에너미는 Start() 대신 InitHealth()를 OnSpawn()에서 호출
    public void InitHealth()
    {
        health = maxHealth;
        IsDie = false; // 사망 상태 초기화
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if (health == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }
        Debug.Log($"{this.gameObject} : {health}");
    }
}