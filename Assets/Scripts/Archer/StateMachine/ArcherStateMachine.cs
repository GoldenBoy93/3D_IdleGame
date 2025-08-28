using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateMachine : StateMachine
{
    public Archer Archer { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; }

    public Health Target { get; private set; }
    public ArcherIdleState IdleState { get; }
    public ArcherChasingState ChasingState { get; }
    public ArcherAttackState AttackState { get; }

    public ArcherStateMachine(Archer archer)
    {
        this.Archer = archer;
        //Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Health>();

        IdleState = new ArcherIdleState(this);
        ChasingState = new ArcherChasingState(this);
        AttackState = new ArcherAttackState(this);

        MovementSpeed = Archer.Data.GroundData.BaseSpeed;
        RotationDamping = Archer.Data.GroundData.BaseRotationDamping;
    }

    // 타겟을 찾는 함수 추가
    public void FindNearestTarget()
    {
        // 씬에 있는 모든 "Enemy" 태그를 가진 오브젝트들을 배열로 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Target = null; // 적이 없으면 타겟을 null로 설정
            return;
        }

        // 가장 가까운 적을 찾기 위한 변수 초기화
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity; // 무한대로 초기화하여 첫 번째 적과 비교 가능하게 함

        foreach (GameObject enemy in enemies)
        {
            // 현재 아처와 적 사이의 거리 계산
            float distance = Vector3.Distance(Archer.transform.position, enemy.transform.position);

            // 현재 적이 가장 가까운 적보다 더 가까우면 타겟을 업데이트
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // 가장 가까운 적을 타겟으로 설정
        if (nearestEnemy != null)
        {
            Target = nearestEnemy.GetComponent<Health>();
        }
        else
        {
            Target = null;
        }
    }
}