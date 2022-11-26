using UnityEngine;

public class GameStateChangeInvoker : MonoBehaviour
{
    [SerializeField] private GameState stateToChangeTo;
    [SerializeField] private RequestGameStateChangeEventChannelSO channel;

    public void RaiseEvent()
    {
        channel.RaiseEvent(stateToChangeTo);
    }
}
