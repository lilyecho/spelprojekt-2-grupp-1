using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFromMovingOnMaterial
{
    //TODO Fixa så att den egentligen är baserad på animation
    /// <summary>
    /// Raycast straight down from object-space and gets one material overall from the other objects component "MaterialCompositionComponent"
    /// </summary>
    /// <param name="materialCheckerTransform"></param>
    /// <returns></returns>
    public static MaterialComposition GetObjectMaterial(Transform materialCheckerTransform)
    {
        Vector3 directionToMaterial = materialCheckerTransform.up * -1;
        if (!Physics.Raycast(materialCheckerTransform.position, directionToMaterial, out RaycastHit hit, 1))
        {
            Debug.Log("Missed");
            return MaterialComposition.None;
        }
        
        try
        {
            return hit.collider.GetComponent<MaterialCompositionComponent>().GetMaterial;
        }
        catch (Exception e)
        {
            Debug.Log(hit.collider.gameObject.name +": Missing the DynamaicMaterial-component"+"\n "+e);
            return MaterialComposition.None;
        }
        
        
    }
}
