using UnityEngine;

public class DynamicMaterial : MonoBehaviour
{
    [SerializeField] private MaterialComposition materialType;

    public MaterialComposition GetMaterial => materialType;
}
