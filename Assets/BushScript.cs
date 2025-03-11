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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach(GameObject bush in bushes)
            {
                bush.GetComponent<MeshRenderer>().material = transparentBushMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject bush in bushes)
            {
                bush.GetComponent<MeshRenderer>().material = opaqueBushMaterial;
            }
        }
    }
}
