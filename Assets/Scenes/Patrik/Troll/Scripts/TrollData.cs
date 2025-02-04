using UnityEngine;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;

    
    [SerializeField] private float hearingRange;

    [Header("Senses")]
    
    [Space,Header("Sight")]
    [SerializeField] private float sightRange;
    [SerializeField] private float sightAngle;
    
    public float GetSightRange => sightRange;
    public float GetSightAngle => sightAngle;
    public float GetHearingRange => hearingRange;
}
