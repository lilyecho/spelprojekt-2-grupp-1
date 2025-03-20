using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class CombineMeshes : MonoBehaviour
{
    void Start()
    {
        //Recommended by documentation
        Vector3 pos = transform.position;
        //Vector3 scale = transform.localScale;
        Quaternion rot = transform.rotation;

        //Recommended by documentation
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        //transform.localScale = Vector3.one;
        
        
        //Documentation solution
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            if(!meshFilters[i].mesh.isReadable) Debug.Log(meshFilters[i].mesh.name);
            
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        gameObject.SetActive(true);
        
        //Recommended by documentation
        transform.position = pos;
        transform.rotation = rot;
        //transform.localScale = scale;

    }
}