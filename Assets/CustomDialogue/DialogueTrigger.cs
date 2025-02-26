using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart = null;
    [SerializeField] private string affectedBlock;

    private void OnTriggerEnter(Collider other)
    {
        Block block = flowchart.FindBlock(affectedBlock);
        block.StartExecution();
        Destroy(gameObject);
    }
}
