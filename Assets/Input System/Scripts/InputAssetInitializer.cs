using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Samples.RebindUI;
using UnityEngine.InputSystem;
public class InputAssetInitializer : MonoBehaviour
{
    public DefaultControlsInstanceAsset asset;

    private void Start()
    {
        asset.CreateInstance();
        asset.InputActionsAsset.Gameplay.Enable();
    }

    private void OnDestroy()
    {
        asset.Dispose();
    }
}
