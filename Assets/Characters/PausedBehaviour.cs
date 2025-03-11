using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedBehaviour : MonoBehaviour
{
    private bool _movementOn = true;
    
    private void ChangeMovementActivation(bool nextValue)
    {
        _movementOn = nextValue;
        //Fungerar som en stopper
        rb.constraints = nextValue ? RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        anim.speed = nextValue ? 1 : 0;
    }
}
