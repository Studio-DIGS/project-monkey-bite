using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

public class RebindSetup : MonoBehaviour
{
    public List<RebindActionUI> rebinders;
    public DefaultControlsInstanceAsset assetInstance;
    public InputActionAsset source;
    public int actionMapIndex;
    [TextArea] public string test;

    public void CopyBindingsToInstance(RebindActionUI a, InputActionRebindingExtensions.RebindingOperation b)
    {
        test = source.SaveBindingOverridesAsJson();
        assetInstance.LoadOverrideBindings(test);
    }
/*
    private void Awake()
    {
        assetInstance.OnInstanceCreated.AddListener(UpdateRebindTargets);
    }

    private void UpdateRebindTargets(DefaultControls controls)
    {

        var actions = controls.asset.actionMaps[actionMapIndex].actions;
        int actionCount = actions.Count;
        for (int i = 0; i < Mathf.Min(actionCount, rebinders.Count); i++)
        {
            var actionRef = new InputActionReference();
            actionRef.Set(actions[i]);
            rebinders[i].actionReference = actionRef;
            rebinders[i].bindingId = actions[i].bindings[0].id.ToString();
        }
    }*/
}
