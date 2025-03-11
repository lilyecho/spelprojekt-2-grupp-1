using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlies : MonoBehaviour
{
    public float speed;
    public Vector3 rotation;
    public float width;
    public float height;
    private float time;


    public GameObject fireFlies;
    Vector3 startPosition;

    private Transform player;
    private float distance;


    private bool moving = false;

    void Start()
    {
        startPosition = fireFlies.transform.localPosition;
        player = GameObject.FindWithTag("Player")?.transform;
        
        

    }
    


    public FireFliesPoint[] points;
    private int index = 0;
    public float moveSpeed;


    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime * speed;
        
        float x = width * Mathf.Sin(time);
        float y = height * Mathf.Sin(time) * Mathf.Cos(time);

        fireFlies.transform.localPosition = startPosition + new Vector3(x, y, 0);
        
        transform.Rotate(rotation * Time.deltaTime);


        distance = Vector3.Distance(transform.position, player.position);

        if (!moving && distance < 3 && index < points.Length)
        {
            moving = true;
            
        }
        if (moving)
        {
            if (index < points.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[index].transform.position, moveSpeed * Time.deltaTime);
            }
            

            if(Vector3.Distance(transform.position, points[index].transform.position) <  0.2f)
            {
                if (points[index].isStop || index == points.Length - 1)
                {
                    moving = false;
                }
                index++;
                
            }
        }
    }

    private void OnValidate()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (points.Length > 1)
        {
            Gizmos.DrawLine(transform.position, points[0].gameObject.transform.position);
            for (int i = 0; i < points.Length -1; i++)
            {
                Gizmos.DrawLine(points[i].gameObject.transform.position, points[i + 1].gameObject.transform.position);
            }

            
        }
    }


}
