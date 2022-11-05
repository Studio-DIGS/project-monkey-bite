using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;


public class RebindSettingsGroup : SettingsGroup
{
    [SerializeField] private BindingOverrideLoader bindingLoader;

    [SerializeField] private List<RebindActionUI> rebindComponents;

    private const string prefsKey = "rebinds";

    public override void LoadSavedSettings()
    {
        bindingLoader.LoadRebindFromPrefs(prefsKey);
        foreach (var rebind in rebindComponents)
            rebind.UpdateBindingDisplay();
    }

    public override void BeginSession()
    {
        bindingLoader.CacheCurrentBindings();
        OnGroupMarkedDirty?.Invoke();
    }

    public override void CloseSession()
    {
        bindingLoader.LoadBindingsFromCache();
    }

    public override void SaveChanges()
    {
        bindingLoader.CacheCurrentBindings();
        bindingLoader.SaveRebindToPrefs(prefsKey);
    }
}

