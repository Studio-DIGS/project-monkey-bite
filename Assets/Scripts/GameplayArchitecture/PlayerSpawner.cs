using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO gameplaySceneReady;

    [SerializeField] private GameObject playerAsset;

    private SpawnLocation location;

    private void Awake()
    {
        location = FindObjectOfType<SpawnLocation>();
    }

    private void OnEnable()
    {
        gameplaySceneReady.OnEventRaised += SpawnPlayer;
    }

    private void OnDisable()
    {
        gameplaySceneReady.OnEventRaised -= SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        Instantiate(playerAsset, location.transform.position, Quaternion.identity);
    }
}
