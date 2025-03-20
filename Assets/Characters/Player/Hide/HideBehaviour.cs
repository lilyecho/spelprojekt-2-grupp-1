using System;
using Characters.Player.PlayerBehaviour;
using UnityEngine;

[RequireComponent(typeof(PlayerBehaviour))]
public class HideBehaviour : MonoBehaviour
{
    [SerializeField] private HidePort hidePort;
    private PlayerBehaviour _playerBehaviour;
    [SerializeField] private int amountOfActiveHideObjects;

    private void OnEnable()
    {
        hidePort.OnHidden += UpdateAmountOfActiveHideObjects;
    }

    private void OnDisable()
    {
        hidePort.OnHidden -= UpdateAmountOfActiveHideObjects;
    }

    private void Awake()
    {
        _playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    private void UpdateAmountOfActiveHideObjects(bool isHiding)
    {
        amountOfActiveHideObjects += isHiding ? 1 : -1;
        _playerBehaviour.Hidden = amountOfActiveHideObjects >= 1;
    }
    
}
