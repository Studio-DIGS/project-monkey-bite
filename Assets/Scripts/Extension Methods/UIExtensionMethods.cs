
using UnityEngine.UI;
using UnityEngine;

public static class UIExtensionMethods
{
    public static void SetVisible(this CanvasGroup group, bool val)
    {
        group.alpha = val ? 1 : 0;
        group.blocksRaycasts = val;
    }
}
