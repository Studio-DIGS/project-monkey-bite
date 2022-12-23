
using UnityEngine.UI;
using UnityEngine;

public static class UIExtensionMethods
{
    public static void SetVisible(this CanvasGroup group, bool val)
    {
        group.alpha = val ? 1 : 0;
        group.blocksRaycasts = val;
    }

    public static void SetAlpha(this Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
}
