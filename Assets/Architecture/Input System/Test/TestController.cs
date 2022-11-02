using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public InputProvider<PlayerInputState> inputProvider;
    public Transform playerTransform;

    private Vector3 pos;

    private void Start()
    {
        inputProvider.GetInputState().OnJumpPressed += () => print("AHHHH Jump :D");
    }

    private void Update()
    {
        PlayerInputState state = inputProvider.GetInputState();
        pos += state.horizontalAxis * Vector3.right * Time.deltaTime * 5f;
        playerTransform.position = pos + Camera.main.ScreenToWorldPoint(new Vector3(state.mousePosition.x, state.mousePosition.y, 5));
    }
}
