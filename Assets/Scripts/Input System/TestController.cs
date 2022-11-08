using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestController : MonoBehaviour
{
    public PlayerUserInputProvider inputPlayerInputProvider;
    public Transform playerTransform;

    private Vector3 pos;

    private void Start()
    {
        inputPlayerInputProvider.Events.OnJumpPressed += () => print("AHHHH Jump :D");
        inputPlayerInputProvider.Events.OnInteractPressed += () => print("OWO INTERACT");
        inputPlayerInputProvider.Events.OnMainAttackPressed += () => print("Whack go to horny jail");
        inputPlayerInputProvider.Events.OnAltAttackPressed += () => print("SHIN DESTROYER");
    }

    private void Update()
    {
        PlayerInputState state = inputPlayerInputProvider.GetInputState();
        pos += Vector3.right * (state.horizontalAxis * Time.deltaTime * 5f);
        playerTransform.position =
            pos + Camera.main.ScreenToWorldPoint(new Vector3(state.mousePosition.x, state.mousePosition.y, 5));
    }

    private IEnumerable<string> Test()
    {
        yield return "AHAHAA";
        yield return "BEES";
        yield return "KNEES";
    }
}
