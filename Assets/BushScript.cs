using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour
{
    public Material opaqueBushMaterial;
    public Material transparentBushMaterial;
    public List<GameObject> bushes;
    
    void Start()
    {
        foreach(Transform child in transform)
        {
            bushes.Add(child.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach(GameObject bush in bushes)
        {
            bush.GetComponent<MeshRenderer>().material = transparentBushMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject bush in bushes)
        {
            bush.GetComponent<MeshRenderer>().material = opaqueBushMaterial;
        }
    }
}
