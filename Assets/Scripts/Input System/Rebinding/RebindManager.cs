using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

public class RebindManager : MonoBehaviour
{

    [Tooltip("Target asset")]
    [SerializeField]
    private InputActionAsset targetAsset;

    private void SetSourceAsset(InputActionAsset asset)
    {
        targetAsset = asset;
    }

    // Move Save/Load out later
    private void LoadRebindJSON()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            targetAsset.LoadBindingOverridesFromJson(rebinds);
    }

    private void SaveRebindJSON()
    {
        var rebinds = targetAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
