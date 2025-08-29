using UnityEngine;

public class ArcherBaseState : IState
{
    protected ArcherStateMachine stateMachine;
    protected readonly ArcherBasicData basicData;

    public ArcherBaseState(ArcherStateMachine archerStateMachine)
    {
        stateMachine = archerStateMachine;
        basicData = stateMachine.Archer.Data.BasicData;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void HandleInput()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        Move();
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Archer.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Archer.Animator.SetBool(animationHash, false);
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    protected Vector3 GetMovementDirection()
    {
        if (stateMachine.Target == null)
        {
            return Vector3.zero;
        }

        Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Archer.transform.position).normalized;
        return dir;
    }

    void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Archer.Controller.Move(((movementDirection * movementSpeed) + stateMachine.Archer.ForceReceiver.Movement) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            stateMachine.Archer.transform.rotation = Quaternion.Lerp(stateMachine.Archer.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void ForceMove()
    {
        stateMachine.Archer.Controller.Move(stateMachine.Archer.ForceReceiver.Movement * Time.deltaTime);
    }

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    protected bool IsInChasingRange()
    {
        // 타겟이 아직없거나(스폰되기 전 등의 이유로 null) || 타겟이 죽었거나 || 공격성향이 아니면 함수 끝(return)
        if (stateMachine.Target == null || stateMachine.Target.IsDie || !stateMachine.Archer.Data.aggressive) 
            return false;

        float EnemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Archer.transform.position).sqrMagnitude;
        return EnemyDistanceSqr <= stateMachine.Archer.Data.EnemyChasingRange * stateMachine.Archer.Data.EnemyChasingRange;
    }

    protected bool IsInAttackRange()
    {
        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Archer.transform.position).sqrMagnitude;

        return enemyDistanceSqr <= stateMachine.Archer.Data.AttackRange * stateMachine.Archer.Data.AttackRange;
    }
}