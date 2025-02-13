using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFromMovingOnMaterial
{
    private MaterialComposition GetObjectMaterial(Transform materialCheckerTransform)
    {
        Vector3 directionToMaterial = materialCheckerTransform.up * -1;
        if (!Physics.Raycast(materialCheckerTransform.position, directionToMaterial, out RaycastHit hit, 1))
        {
            Debug.Log("Missed");
            return MaterialComposition.None;
        }


        try
        {
            return hit.collider.GetComponent<DynamicMaterial>().GetMaterial;
        }
        catch (Exception e)
        {
            Debug.Log(hit.collider.gameObject.name +": Missing the DynamaicMaterial-component");
            return MaterialComposition.None;
        }
        
        
    }
}
