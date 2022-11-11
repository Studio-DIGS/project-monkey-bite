using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestController : MonoBehaviour
{
    public PlayerUserInputProvider inputPlayerInputProvider;
    public Transform playerTransform;

    private Vector3 pos;

    private PlayerInputState _state;

    private void Start()
    {
        inputPlayerInputProvider.Events.OnJumpPressed += () => print("AHHHH Jump :D");
        inputPlayerInputProvider.Events.OnInteractPressed += () => print("OWO INTERACT");
        inputPlayerInputProvider.Events.OnMainAttackPressed += () => print("Whack go to horny jail");
        inputPlayerInputProvider.Events.OnAltAttackPressed += () => print("SHIN DESTROYER");
    }

    private void Update()
    {
        inputPlayerInputProvider.GetInputState(ref _state);
        pos += Vector3.right * (_state.horizontalAxis * Time.deltaTime * 5f);
        playerTransform.position =
            pos + Camera.main.ScreenToWorldPoint(new Vector3(_state.mousePosition.x, _state.mousePosition.y, 5));
    }
}
