using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    [SerializeField] private AudioPort audioPort = null;
    [SerializeField] private FmodParameterData parameters = null;
    
    [Header("Sfx-Related")]
    [SerializeField] private AudioHandler audioHandler = null;

    private void OnEnable()
    {
        audioPort.OnStep += CreateSound4Step;
        audioPort.OnJump += CreateSound4Jump;
    }

    private void OnDisable()
    {
        audioPort.OnStep -= CreateSound4Step;
        audioPort.OnJump -= CreateSound4Jump;
        
    }

    private void CreateSound4Step(CharacterAudioData characterAudioData, Transform checkerTransform)
    {
        MaterialComposition material = SoundFromMovingOnMaterial.GetObjectMaterial(checkerTransform);
        
        Dictionary<string, float> parameternamesAndValues = new Dictionary<string,float>
            {
                [parameters.GetMaterialParameter] = (float)material
            };
        
        audioHandler.PlayOneShot(
            characterAudioData.GetAudioMovement,checkerTransform.position,parameternamesAndValues);
    }

    private void CreateSound4Jump(EventReference eventReference, Vector3 jumpPos)
    {
        audioHandler.PlayOneShot(eventReference, jumpPos);
    }
}
