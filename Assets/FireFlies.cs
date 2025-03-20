using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FireFlies : MonoBehaviour
{
    public float speed;
    public Vector3 rotation;
    public float width;
    public float height;
    private float time;

    public GameObject fireFliesParent;
    public GameObject fireFlies;
    Vector3 startPosition;

    Vector3 startPositionWorld;

    private Transform player;
    private float distance;

    public float triggerRadius;


    private bool moving = false;
    [HideInInspector]public bool hasReachedEndOfPath;

    void Start()
    {
        fireFlies.transform.localPosition = Vector3.zero;
        startPosition = fireFlies.transform.localPosition;
        startPositionWorld = fireFliesParent.transform.position;
        player = GameObject.FindWithTag("Player")?.transform;
    }
    


    public FireFliesPoint[] points;
    public GameObject pointsParentObject;
    private int index = 0;
    public float moveSpeed;


    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime * speed;
        
        float x = width * Mathf.Sin(time);
        float y = height * Mathf.Sin(time) * Mathf.Cos(time);

        fireFlies.transform.localPosition = startPosition + new Vector3(x, y, 0);
        
        //transform.Rotate(rotation * Time.deltaTime);
        fireFlies.transform.Rotate(rotation * Time.deltaTime);

        if(player != null)
        {
            distance = Vector3.Distance(fireFliesParent.transform.position, player.position);
        }
        

        if (!moving && distance < triggerRadius && index < points.Length)
        {
            moving = true;
            
            
        }
        if (moving)
        {
            if (index < points.Length)
            {
                //transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, moveSpeed * Time.deltaTime);
                fireFliesParent.transform.position = Vector3.MoveTowards(fireFliesParent.transform.position, points[index].transform.position, moveSpeed * Time.deltaTime);
            }
            
            /*
            if(Vector3.Distance(transform.position, points[index].transform.position) <  0.2f)
            {
                if (points[index].isStop || index == points.Length - 1)
                {
                    moving = false;
                }
                index++;
                
            }
            */
            if (Vector3.Distance(fireFliesParent.transform.position, points[index].transform.position) < 0.2f)
            {
                if (points[index].isStop || index == points.Length - 1)
                {
                    triggerRadius = points[index].distanceToTrigger;
                    moveSpeed = points[index].changeSpeedTo;
                    moving = false;
                }
                index++;

            }
        }
    }

    private void OnValidate()
    {
        points = new FireFliesPoint[pointsParentObject.transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = pointsParentObject.transform.GetChild(i).GetComponent<FireFliesPoint>();
            points[i].index = i;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (points.Length > 0)
        {
            Gizmos.DrawLine(fireFliesParent.transform.position, points[0].gameObject.transform.position);

            if(points.Length > 1)
            {
                for (int i = 0; i < points.Length - 1; i++)
                {
                    Gizmos.DrawLine(points[i].gameObject.transform.position, points[i + 1].gameObject.transform.position);
                    Gizmos.DrawWireSphere(fireFliesParent.transform.position, triggerRadius);
                    if (points[i].isStop)
                    {
                        Gizmos.DrawWireSphere(points[i].gameObject.transform.position, points[i].distanceToTrigger);
                    }
                }
            }
            

            
        }
    }

    private void OnTransformChildrenChanged()
    {
        
        OnValidate();
    }

    
    public void StartResetPosition()
    {
        StartCoroutine(ResetPosition());
    }

    public IEnumerator ResetPosition()
    {
        yield return null;

        float dist = float.MaxValue;
        FireFliesPoint spawnPoint = null;

        for(int i = 0;i < points.Length;i++)
        {
            if (Vector2.Distance(points[i].transform.position, player.transform.position) < dist)
            {
                dist = Vector2.Distance(points[i].transform.position, player.transform.position);
                spawnPoint = points[i];
            }
        }


        moving = false;
        index = spawnPoint.index;
        //fireFliesParent.transform.position = startPositionWorld;
        if(spawnPoint != null)
        {
            fireFliesParent.transform.position = spawnPoint.transform.position;
        }
        
    }

    public CheckPointPort checkPointPort;

    private void OnEnable()
    {
        checkPointPort.OnRespawn += StartResetPosition;
    }
    private void OnDisable()
    {
        checkPointPort.OnRespawn -= StartResetPosition;
    }

}
