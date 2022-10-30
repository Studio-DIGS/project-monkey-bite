using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public PlayerInputProvider inputProvider;
    public Transform playerTransform;

    private void Awake()
    {
        inputProvider.Setup();
        inputProvider.OnJumpReleased += () => print("AHHHH");
    }

    private void OnDestroy()
    {
        inputProvider.Cleanup();
    }

    private void Update()
    {
        PlayerInputState state = inputProvider.GetInputState();
        playerTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(state.mousePosition.x, state.mousePosition.y, 5));
    }
}
