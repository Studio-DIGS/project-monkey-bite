using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Handles spawning the player into the game level, including any animations or cutscenes before the player can enter gameplay
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    [ColorHeader("Listening - On Level Scene Ready Channel", ColorHeaderColor.ListeningEvents)]
    [SerializeField] private VoidEventChannelSO onLevelSceneReady;

    [ColorHeader("Invoking - Player Gameplay Ready Channels", ColorHeaderColor.TriggeringEvents)] 
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private GameObject playerAsset;

    private SpawnLocation location;

    private void Awake()
    {
        location = FindObjectOfType<SpawnLocation>();
    }

    private void OnEnable()
    {
        onLevelSceneReady.OnRaised += SpawnPlayer;
    }

    private void OnDisable()
    {
        onLevelSceneReady.OnRaised -= SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        Instantiate(playerAsset, location.transform.position, Quaternion.identity);
        askInputStateChange.RaiseEvent(InputState.Gameplay);
    }
}
