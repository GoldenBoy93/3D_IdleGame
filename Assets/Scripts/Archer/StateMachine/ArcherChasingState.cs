public class ArcherChasingState : ArcherBaseState
{
    public ArcherChasingState(ArcherStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1;
        base.Enter();
        StartAnimation(stateMachine.Archer.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Archer.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Archer.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Archer.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!IsInChasingRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }

    protected bool IsInAttackRange()
    {
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Archer.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.Archer.Data.AttackRange * stateMachine.Archer.Data.AttackRange;
    }
}