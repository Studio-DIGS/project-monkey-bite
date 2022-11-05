using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestController : MonoBehaviour
{
    public PInputStateProviderSO inputProvider;
    public Transform playerTransform;

    private Vector3 pos;

    private void Start()
    {
        inputProvider.OnJumpPressed += () => print("AHHHH Jump :D");
        inputProvider.OnInteractPressed += () => print("OWO INTERACT");
        inputProvider.OnMainAttackPressed += () => print("Whack go to horny jail");
        inputProvider.OnAltAttackPressed += () => print("SHIN DESTROYER");
    }

    private void Update()
    {
        PlayerInputState state = inputProvider.GetInputState();
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
