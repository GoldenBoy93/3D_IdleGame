using UnityEngine;

[CreateAssetMenu(fileName = "Archer", menuName = "Characters/Archer")]
public class ArcherSO : ScriptableObject
{
    [field: SerializeField] public float EnemyChasingRange { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage;
    [field: SerializeField] public PlayerGroundData GroundData { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_Start_TransitionTime { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float Dealing_End_TransitionTime { get; private set; }

    [field: SerializeField] public bool aggressive = true;
}