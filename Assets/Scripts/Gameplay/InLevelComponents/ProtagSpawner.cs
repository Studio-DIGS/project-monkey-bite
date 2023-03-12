using System;
using System.Collections;
using System.Collections.Generic;
using MushiCore.EditorAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// Handles spawning the player into the game level, including any animations or cutscenes before the player can enter gameplay
/// </summary>
public class ProtagSpawner : DescriptionMonoBehavior
{
    [ColorHeader("Listening", ColorHeaderColor.ListeningChannels)]
    [ColorHeader("On Level Scene Ready")]
    [SerializeField] private VoidEventChannelSO onLevelSceneReady;

    [ColorHeader("Invoking", ColorHeaderColor.InvokingChannels)] 
    [ColorHeader("Ask Change Input State")]
    [SerializeField] private InputStateEventChannelSO askInputStateChange;

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)]
    [SerializeField] private GameObject playerAsset;

    private SpawnLocation location;

    private void Awake()
    {
        SpawnLocation location = null;
        while (!location)
        {
            location = FindObjectOfType<SpawnLocation>();
            if (!location.isActiveAndEnabled)
                location = null;
        }
       
        if (!location)
        {
            Debug.LogError("Missing Level Spawn Location, unable to spawn player");
        }
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
        var protag = Instantiate(playerAsset, Vector3.zero, Quaternion.identity);
        protag.GetComponentInChildren<Protagonist>().Initialize(location.transform.position);
        askInputStateChange.RaiseEvent(InputState.Gameplay);
    }
}
