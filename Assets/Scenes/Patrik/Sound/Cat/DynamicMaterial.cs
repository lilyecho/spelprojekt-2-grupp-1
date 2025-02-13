using UnityEngine;

public class DynamicMaterial : MonoBehaviour
{
    public enum MaterialTypes
    {
        Gras,
        Wood,
        Stone
    }

    [SerializeField] private MaterialTypes materialType;

    public MaterialTypes GetMaterial => materialType;
}
