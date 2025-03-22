using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class CheckPointBehaviour : MonoBehaviour
{
    [SerializeField, ReadOnly] private AbilityData.Abilities savedAbilities;
    [SerializeField] private Vector3 spawnPoint = Vector3.zero;
    public UnityEvent changesOnStart;

    [SerializeField] private BoxCollider checkCollider = null;
    [SerializeField] private Color visualizerColor = Color.black;

    public Vector3 SpawnPoint
    {
        get => transform.position + spawnPoint;
        set => spawnPoint = value - transform.position;
    }

    public AbilityData.Abilities Abilities
    {
        get => savedAbilities;
        set => savedAbilities = value;
    }

    private void Awake()
    {
        checkCollider = GetComponent<BoxCollider>();
    }
    
    private void Start()
    {
        changesOnStart.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = visualizerColor;
        Gizmos.DrawWireCube(checkCollider.transform.position, checkCollider.size);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position + spawnPoint, new Vector3(.1f,.1f,.1f));
    }
}


