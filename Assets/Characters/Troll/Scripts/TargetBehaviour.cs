using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Could be removed and implemented in player movement or likewise script
public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] private TargetPort targetPort = null;
    // Start is called before the first frame update
    void Start()
    {
        targetPort.OnTargetCreated(gameObject);
    }
    
}
