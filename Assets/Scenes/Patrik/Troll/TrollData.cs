using UnityEngine;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private float sightRange;
    [SerializeField] private float hearingRange;



    public float GetSightRange => sightRange;
    public float GetHearingRange => hearingRange;
}
