using System;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Architecture/Input/DefaultControlsInstanceAsset")]
public class DefaultControlsInstanceAsset : ScriptableObject
{
    private DefaultControls inputActionsAsset;
    public DefaultControls InputActionsAsset
    {
        get { return inputActionsAsset; }
    }

    public UnityEvent<DefaultControls> OnInstanceCreated;

    public void CreateInstance()
    {
        if(inputActionsAsset != null)
        {
            inputActionsAsset.Dispose();
        }
        inputActionsAsset = new DefaultControls();
        OnInstanceCreated?.Invoke(inputActionsAsset);
    }

    public void Dispose()
    {
        inputActionsAsset.Dispose();
        inputActionsAsset = null;
    }

    public void LoadOverrideBindings(string jsonSource)
    {
        inputActionsAsset.Disable();
        inputActionsAsset.LoadBindingOverridesFromJson(jsonSource);
        inputActionsAsset.Gameplay.Enable();
    }
}
