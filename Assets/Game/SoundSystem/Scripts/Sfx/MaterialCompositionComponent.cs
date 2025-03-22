using UnityEngine;

public class MaterialCompositionComponent : MonoBehaviour
{
    [SerializeField] private MaterialComposition materialType;

    public MaterialComposition GetMaterial => materialType;
}
