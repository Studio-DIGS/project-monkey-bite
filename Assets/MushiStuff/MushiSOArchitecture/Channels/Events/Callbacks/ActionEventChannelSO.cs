#region

using System;
using UnityEngine;

#endregion

[CreateAssetMenu(menuName = "Channels/Events/Callbacks/ActionEventChannel", fileName = "NewActionEventChannel")]
public class ActionEventChannelSO : GenericEventChannelSO<Action>
{
}