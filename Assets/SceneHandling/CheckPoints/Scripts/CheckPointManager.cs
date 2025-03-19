using Unity.Collections;
using UnityEngine;

namespace SceneHandling.CheckPoints.Scripts
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private CheckPointPort checkPointPort = null;
        [SerializeField] private RegistrationPort registrationPort = null;
        [SerializeField]private PlayerBehaviour playerBehaviour = null;
    
        [SerializeField,ReadOnly] private CheckPointBehaviour latestCheckPoint = null;
    
        private void Awake()
        {
            registrationPort.OnRegister += Registration;
        
            checkPointPort.OnChangeCheckPoint += ChangeLatestCheckPoint;
            checkPointPort.OnRespawn += Respawn;
            //checkPointPort.OnChangeCheckPoint += ChangeLatestAbilities;
        }

        private void ChangeLatestCheckPoint(CheckPointBehaviour checkPoint)
        {
            latestCheckPoint = checkPoint;
        
            if (playerBehaviour == null) return;
            latestCheckPoint.Abilities = playerBehaviour.GetAbilities;
        }
    
        private void Registration(RegistrationPort.TypeOfRegistration typeOfRegistration, GameObject gameObject)
        {
            if (typeOfRegistration != RegistrationPort.TypeOfRegistration.Player) return;
            if (!gameObject.TryGetComponent(out PlayerBehaviour player)) return;
            playerBehaviour = player;
        }

        private void Respawn()
        {
            playerBehaviour.transform.position = latestCheckPoint.SpawnPoint;
            playerBehaviour.ResetAbilities = latestCheckPoint.Abilities;
            playerBehaviour.Attacked = false;
            playerBehaviour.ChangeMovementActivation(true);
        }
    }
}
