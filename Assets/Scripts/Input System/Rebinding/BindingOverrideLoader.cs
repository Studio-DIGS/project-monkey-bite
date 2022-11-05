using UnityEngine;
using UnityEngine.InputSystem;


public class BindingOverrideLoader : MonoBehaviour
{
    [Tooltip("Target asset")] [SerializeField]
    private InputActionAsset targetAsset;

    private string cache = string.Empty;

    public void CacheCurrentBindings()
    {
        cache = targetAsset.SaveBindingOverridesAsJson();
    }

    public void LoadBindingsFromCache()
    {
        targetAsset.LoadBindingOverridesFromJson(cache);
    }

    public void LoadRebindFromPrefs(string key)
    {
        var rebinds = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(rebinds))
            targetAsset.LoadBindingOverridesFromJson(rebinds);
    }

    public void SaveRebindToPrefs(string key)
    {
        var rebinds = targetAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(key, rebinds);
    }
}
