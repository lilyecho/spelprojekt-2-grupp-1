using System;
using UnityEngine;

public sealed class PhysicsCalculations
{
    public static float ForceToJumpCertainHeight(float heightValue, float objectMass, float gravityMagnitude)
    {
        return objectMass * MathF.Sqrt(heightValue * 2f * gravityMagnitude);
    }

    /// <summary>
    /// Not adapted for frames
    /// </summary>
    /// <returns></returns>
    public static float ProjectileCertainTimeOfMaxHeight(float startVelocity, float angleOfIncrease, float gravitationValue)
    {
        return startVelocity * Mathf.Sin(angleOfIncrease*Mathf.Deg2Rad)/gravitationValue;
    }
    
    /// <summary>
    /// Not adapted for frames
    /// </summary>
    /// <returns></returns>
    public static float ProjectileCertainWidthFromRealTime(float startVelocity, float angleOfIncrease, float realTime)
    {
        return startVelocity * realTime * Mathf.Cos(angleOfIncrease*Mathf.Deg2Rad);
    }
}
