using UnityEngine;

[CreateAssetMenu(menuName = "Troll/TrollData")]
public class TrollData : ScriptableObject
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
}
