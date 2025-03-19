using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FakeLightCone : MonoBehaviour
{
    [SerializeField] private Transform troll;
    [SerializeField] private TrollData trollData;
    [SerializeField, Min(4)] private int resolutionAreaMarkers=4;

    private MeshFilter _meshFilter;
    
    private void OnValidate()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }
    
    private void UpdateLightCone()
    {
        Vector3 direction = troll.forward;
        float angleBetweenPoints = 360f / resolutionAreaMarkers;
        Vector3[] edgeVertices =
            GetFurthestPoints(transform.position, direction, resolutionAreaMarkers, angleBetweenPoints);
        
        Gizmos.color = Color.red;
        foreach (Vector3 point in edgeVertices)
        {
            Gizmos.DrawCube(point, new Vector3(0.1f,0.1f,0.1f));
        }

        List<Mesh> meshes = new List<Mesh>(); 
        //ConeFormation
        meshes.Add(CreateConeMesh(transform.position, edgeVertices));

        _meshFilter.mesh = meshes[0];

        //CenterFormation
        Vector3 centerPoint = CreateCenterPoint();
        meshes.Add(CreatePizzaMesh(centerPoint, edgeVertices));

        //Recommended by documentation
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        //Recommended by documentation
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        
        CombineInstance[] combine = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            combine[i].mesh = meshes[i];
            combine[i].transform = transform.worldToLocalMatrix;
        }
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine, true);
        //mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
        
        _meshFilter.mesh = mesh;

        //Recommended by documentation
        transform.position = pos;
        transform.rotation = rot;

    }

    private Vector3 CreateCenterPoint()
    {
        if (Physics.Raycast(transform.position, troll.forward, out RaycastHit hit, trollData.GetLampSight.range))
        {
            return hit.point;
        }

        return transform.position + troll.forward * trollData.GetLampSight.range;
    }
    
    private Mesh CreateConeMesh(Vector3 startPoint, Vector3[] edgeVertices)
    {
        Mesh mesh = new Mesh();
        Vector3[] allVert = new Vector3[edgeVertices.Length + 1];
        allVert[0] = transform.InverseTransformPoint(startPoint);
        for (int i = 1; i < allVert.Length; i++)
        {
            allVert[i] =  transform.InverseTransformPoint(edgeVertices[i-1]);
        }

        int[] triangles = new int[3*edgeVertices.Length];
        
        for (int i = 0; i < edgeVertices.Length; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3+2] = (i+1)% edgeVertices.Length+1;
            triangles[i * 3+1] = (i+2)% edgeVertices.Length+1;
        }
        
        mesh.vertices = allVert;
        mesh.triangles = triangles;
        
        return mesh;
    }
    
    private Mesh CreatePizzaMesh(Vector3 startPoint, Vector3[] edgeVertices)
    {
        Mesh mesh = new Mesh();
        Vector3[] allVert = new Vector3[edgeVertices.Length + 1];
        allVert[0] = transform.InverseTransformPoint(startPoint);
        for (int i = 1; i < allVert.Length; i++)
        {
            allVert[i] =  transform.InverseTransformPoint(edgeVertices[i-1]);
        }

        int[] triangles = new int[3*edgeVertices.Length];
        
        for (int i = 0; i < edgeVertices.Length; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3+1] = (i+1)% edgeVertices.Length+1;
            triangles[i * 3+2] = (i+2)% edgeVertices.Length+1;
        }
        
        mesh.vertices = allVert;
        mesh.triangles = triangles;
        
        return mesh;
    }
    
    private Vector3[] GetFurthestPoints(Vector3 startPoint,Vector3 startDirection, int resolution, float angleBetweenPoints)
    {
        List<Vector3> result = new List<Vector3>();
        
        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(trollData.GetLampSight.angle, Vector3.up);
            Quaternion rotationForward = Quaternion.AngleAxis(angleCurrent, startDirection);
            
            Vector3 direction = (rotationForward * rotationY * startDirection).normalized;

            if (Physics.Raycast(startPoint, direction,out RaycastHit hit, trollData.GetLampSight.range))
            {
                result.Add(hit.point);
            }
            else
            {
                result.Add(startPoint+direction*trollData.GetLampSight.range);
            }
        }
        
        return result.ToArray();
    }
    
    private Vector3[] GetFurthestPointsGizmo(Vector3 startPoint,Vector3 startDirection, int resolution, float angleBetweenPoints)
    {
        List<Vector3> result = new List<Vector3>();
        
        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(trollData.GetLampSight.angle, Vector3.up);
            Quaternion rotationForward = Quaternion.AngleAxis(angleCurrent, startDirection);
            
            
            Vector3 newVector = rotationForward * rotationY * (startDirection * trollData.GetLampSight.range);
            
            newVector += startPoint;
            result.Add(newVector);
        }
        
        return result.ToArray();
    }

    private void OnDrawGizmos()
    {
        //UpdateLightConeGizmo();
        UpdateLightCone();
    }
}
