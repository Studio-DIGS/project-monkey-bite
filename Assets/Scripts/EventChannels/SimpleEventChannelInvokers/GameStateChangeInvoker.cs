using UnityEngine;

public class GameStateChangeInvoker : MonoBehaviour
{
    [SerializeField] private GameState stateToChangeTo;
    [SerializeField] private GameStateEventChannelSO channel;

    public void RaiseEvent()
    {
        channel.RaiseEvent(stateToChangeTo);
    }
}
