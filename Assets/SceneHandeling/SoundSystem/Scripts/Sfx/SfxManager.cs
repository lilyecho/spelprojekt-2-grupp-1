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
    }

    private void OnDisable()
    {
        audioPort.OnStep -= CreateSound4Step;
    }

    private void CreateSound4Step(CharacterAudioData characterAudioData, MaterialComposition material, Vector3 position)
    {
        audioHandler.PlayOneShot(
            characterAudioData.GetAudioMovement,
            position,parameters.GetMaterialParameter,
            (int)material);
    }
}
