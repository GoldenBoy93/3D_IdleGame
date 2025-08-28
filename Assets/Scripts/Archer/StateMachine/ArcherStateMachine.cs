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
        Target = GameObject.FindGameObjectWithTag("TowerDoor").GetComponent<Health>();

        IdleState = new ArcherIdleState(this);
        ChasingState = new ArcherChasingState(this);
        AttackState = new ArcherAttackState(this);

        MovementSpeed = Archer.Data.GroundData.BaseSpeed;
        RotationDamping = Archer.Data.GroundData.BaseRotationDamping;
    }
}