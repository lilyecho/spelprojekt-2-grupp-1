using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CombineMeshes : MonoBehaviour
{
    void Start()
    {
        //Recommended by documentation
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        //Recommended by documentation
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        
        
        //Documentation solution
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>()[1..];
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if(!meshFilters[i].mesh.isReadable) Debug.Log(meshFilters[i].mesh.name);
            
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine, true);
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.SetActive(true);
        
        //Recommended by documentation
        transform.position = pos;
        transform.rotation = rot;

    }
}