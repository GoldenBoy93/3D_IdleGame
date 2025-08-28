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

    // Ÿ���� ã�� �Լ� �߰�
    public void FindNearestTarget()
    {
        // ���� �ִ� ��� "Enemy" �±׸� ���� ������Ʈ���� �迭�� ������
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Target = null; // ���� ������ Ÿ���� null�� ����
            return;
        }

        // ���� ����� ���� ã�� ���� ���� �ʱ�ȭ
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity; // ���Ѵ�� �ʱ�ȭ�Ͽ� ù ��° ���� �� �����ϰ� ��

        foreach (GameObject enemy in enemies)
        {
            // ���� ��ó�� �� ������ �Ÿ� ���
            float distance = Vector3.Distance(Archer.transform.position, enemy.transform.position);

            // ���� ���� ���� ����� ������ �� ������ Ÿ���� ������Ʈ
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        // ���� ����� ���� Ÿ������ ����
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