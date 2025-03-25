using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehaviour : MonoBehaviour
{
    [SerializeField] private RegistrationPort registrationPort = null;
    private PlayerBehaviour player = null;
    
    [SerializeField] private AbilityData.Abilities activateAbility;
    

    private void OnEnable()
    {
        registrationPort.OnRegisterStart += RegisterPlayer;
    }

    private void OnDisable()
    {
        registrationPort.OnRegisterStart -= RegisterPlayer;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        player.ChangeAbilities = activateAbility;
        Destroy(gameObject);
    }

    private void RegisterPlayer(RegistrationPort.TypeOfRegistration type, GameObject target)
    {
        if (type != RegistrationPort.TypeOfRegistration.Player) return;
        player = target.GetComponent<PlayerBehaviour>();
    }
}
