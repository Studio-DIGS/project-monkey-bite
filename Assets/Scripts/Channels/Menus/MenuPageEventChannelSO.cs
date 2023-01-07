using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Channels/Menus/Menu Page Event Channel")]
public class MenuPageEventChannelSO : DescriptionBaseSO
{
    public Action<MenuPage> OnRaised;

    public void RaiseEvent(MenuPage page)
    {
        OnRaised?.Invoke(page);
    }
}
