using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FakeLightCone : MonoBehaviour
{
    [SerializeField] private Transform troll;
    [SerializeField] private TrollData trollData;
    [FormerlySerializedAs("resolutionLine")] [SerializeField, Min(4)] private int resolutionPointsInCircle=4;
    [FormerlySerializedAs("resolutionPerLine")] [SerializeField, Min(1)] private int resolutionCircles=1;

    private MeshFilter _meshFilter;

    private Vector3[] rayCastPoints;
    
    private void OnValidate()
    {
        _meshFilter = GetComponent<MeshFilter>();
        ResetFakeLight();
    }

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        ResetFakeLight();
    }

    private void ResetFakeLight()
    {
        //Each line has a certain amount of points, + (startPoint & endPoint - of light)
        rayCastPoints = new Vector3[resolutionPointsInCircle * resolutionCircles + 2];
    }
    
    /*private void UpdateLightCone()
    {
        Vector3 direction = troll.forward;
        float angleBetweenPoints = 360f / resolutionLine;
        Vector3[] edgeVertices =
            RayCastPointsInACircle(transform.position, direction, resolutionLine, angleBetweenPoints);
        
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

    }*/

    private Vector3 CreateFurthestCenterPoint()
    {
        LayerMask layerMask = ~LayerMask.GetMask("Player","InteractiveEnvironment", "Ignore Raycast");
        if (Physics.Raycast(transform.position, troll.forward, out RaycastHit hit, trollData.GetLampSight.range,layerMask))
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
    
    private Vector3[] RayCastPointsInACircle(Vector3 startPoint,Vector3 startDirection, float maxRange, float angleOfSpread, int resolution, float angleBetweenPoints)
    {
        List<Vector3> result = new List<Vector3>();
        
        for (int i = 0; i < resolution; i++)
        {
            float angleCurrent = angleBetweenPoints * i;
            
            Quaternion rotationY = Quaternion.AngleAxis(angleOfSpread, Vector3.up);
            Quaternion rotationForward = Quaternion.AngleAxis(angleCurrent, startDirection);
            
            Vector3 direction = (rotationForward * rotationY * startDirection).normalized;

            if (Physics.Raycast(startPoint, direction,out RaycastHit hit, maxRange))
            {
                result.Add(hit.point);
            }
            else
            {
                result.Add(startPoint+direction*maxRange);
            }
        }
        
        return result.ToArray();
    }


    /// <summary>
    /// Creates rings in the dictionary. The outer ring has the highest key-value and the closest ring to the center has 0. Made in that wha so it has gradiant values. 0,1,2,3,4 osv
    /// </summary>
    /// <returns></returns>
    private Dictionary<int, Vector3[]> RayCastPointsInCircles()
    {
        Dictionary<int, Vector3[]> dict = new Dictionary<int, Vector3[]>();
        
        Vector3 direction = troll.forward;
        float angleBetweenPoints = 360f / resolutionPointsInCircle;
        float increaseSpreadAngle = trollData.GetLampSight.angle / resolutionCircles;
        
        //Will sen raycast in a rings around a certain point 
        for (int i = 0; i < resolutionCircles; i++)
        {
            //New angle for each new ring
            float currentSpread = increaseSpreadAngle * (i+1);
            Vector3[] corners = RayCastPointsInACircle(transform.position, direction,trollData.GetLampSight.range,currentSpread, resolutionPointsInCircle, angleBetweenPoints);
            dict[i] = corners;
        }
        
        return dict;
    }
    
    private void ModernUpdateLightCone()
    {
        Dictionary<int, Vector3[]> dict = RayCastPointsInCircles();
        
        /*Gizmos.color = Color.red;
        foreach (Vector3[] circlesPoints in dict.Values)
        {
            foreach (Vector3 circlePoint in circlesPoints)
            {
                Gizmos.DrawCube(circlePoint, new Vector3(0.1f,0.1f,0.1f));
            }
        }*/

        List<Mesh> meshes = new List<Mesh>(); 
        //ConeFormation
        meshes.Add(ModernCreateConeMesh(transform.position, dict[dict.Count-1]));
        
        //Major front part of the fake-light
        meshes.Add(ModernFakeLightFrontMajorMesh(dict));
        
        //Tie the sack, centerpiece
        Vector3 furthestCenterPoint = CreateFurthestCenterPoint();
        //Gizmos.DrawCube(furthestCenterPoint, new Vector3(0.1f,0.1f,0.1f));
        meshes.Add(ModernFakeLightFrontMinorMesh(furthestCenterPoint, dict[0]));

        _meshFilter.mesh = CombineMeshes(meshes);
        
    }

    private Mesh CombineMeshes(List<Mesh> meshes)
    {
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
        
        //Recommended by documentation
        transform.position = pos;
        transform.rotation = rot;
        
        return mesh;
    }
    
    private Mesh ModernCreateConeMesh(Vector3 startPoint, Vector3[] edgeCorners)
    {
        Mesh mesh = new Mesh();
        List<Vector3> allVertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //TODO can be calculated from triangles
        List<Vector3> normals = new List<Vector3>();
        for (int i = 0; i < edgeCorners.Length; i++)
        {
            CreateTriangle(
                transform.InverseTransformPoint(startPoint), transform.InverseTransformPoint(edgeCorners[(i+1)% edgeCorners.Length]), transform.InverseTransformPoint(edgeCorners[i]),
                ref allVertices, ref triangles,ref normals);
        }

        mesh.vertices = allVertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        
        return mesh;
    }
    
    /// <summary>
    /// Creates from circle zero and upward
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    private Mesh ModernFakeLightFrontMajorMesh(Dictionary<int, Vector3[]> dict)
    {
        Mesh mesh = new Mesh();
        List<Vector3> allVertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //TODO can be calculated from triangles
        List<Vector3> normals = new List<Vector3>();
        
        for (int i = 0; i < resolutionCircles-1; i++)
        {
            Vector3[] innerCircle = dict[i];
            Vector3[] outerCircle = dict[i+1];
            for (int j = 0; j < resolutionPointsInCircle; j++)
            {
                //Variant 1
                CreateTriangle(
                    transform.InverseTransformPoint(innerCircle[j]),transform.InverseTransformPoint(outerCircle[j]), transform.InverseTransformPoint(outerCircle[(j+1)% resolutionPointsInCircle])
                    ,ref allVertices, ref triangles,ref normals);
                
                //Variant 2
                CreateTriangle(
                    transform.InverseTransformPoint(innerCircle[j]),transform.InverseTransformPoint(outerCircle[(j+1)% resolutionPointsInCircle]), transform.InverseTransformPoint(innerCircle[(j+1)% resolutionPointsInCircle])
                    ,ref allVertices, ref triangles,ref normals);
            }
        }
        
        mesh.vertices = allVertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        
        return mesh;
    }
    
    private Mesh ModernFakeLightFrontMinorMesh(Vector3 startPoint, Vector3[] innerCirclePoints)
    {
        Mesh mesh = new Mesh();
        List<Vector3> allVertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        //TODO can be calculated from triangles
        List<Vector3> normals = new List<Vector3>();
        for (int i = 0; i < innerCirclePoints.Length; i++)
        {
            CreateTriangle(
                transform.InverseTransformPoint(startPoint), transform.InverseTransformPoint(innerCirclePoints[i]),transform.InverseTransformPoint(innerCirclePoints[(i+1)% resolutionPointsInCircle]),
                ref allVertices, ref triangles,ref normals);
        }

        mesh.vertices = allVertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        
        return mesh;
    }
    
    private void CreateTriangle(Vector3 start, Vector3 middle, Vector3 end, ref List<Vector3> allVertices, ref List<int> triangles, ref List<Vector3> normals)
    {
        Vector3 normalValue = CalculateNormalValueOfTriangle(start, middle, end);
        
        allVertices.Add(start);
        triangles.Add(allVertices.Count-1);
        normals.Add(normalValue);
        
        allVertices.Add(middle);
        triangles.Add(allVertices.Count-1);
        normals.Add(normalValue);
        
        allVertices.Add(end);
        triangles.Add(allVertices.Count-1);
        normals.Add(normalValue);
    }

    private Vector3 CalculateNormalValueOfTriangle(Vector3 start, Vector3 middle, Vector3 end)
    {
        Vector3 dirMiddleStart = (start - middle).normalized;
        Vector3 dirEndStart = (end - middle).normalized;
        
        return Vector3.Cross(dirEndStart,dirMiddleStart).normalized;
    }

    private void OnDrawGizmos()
    {
        
    }

    private void Update()
    {
        ModernUpdateLightCone();
    }
    
    
}
