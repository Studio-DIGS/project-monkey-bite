using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : BasicCharacter {
    // Member variables
    // ----------------------------------------------------------------------------
    // protected component references
    public PlayerUserInputProvider inputProvider;

    // public constants
    public const float gravity = -10f;

    // public inspector fields
    public float moveSpeed = 10f;
    public float acceleration = 5f;
    public float jumpStrength = 20f;
    public float jumpTime = 1f;

    // private members
    private bool _jumping;
    private float _lastJumpTime;

    // Update is called once per frame
    void Update() {
        PlayerInputState state = inputProvider.GetInputState();
        
        movement.x = state.horizontalAxis * moveSpeed;
        movement.y += gravity * Time.deltaTime;

        if (controller.OnGround()) {
            Debug.Log("GROUND");
            movement.y = Mathf.Max(movement.y, -1f);
            if (state.jumpHeld) {
                _jumping = true;
                _lastJumpTime = Time.time;
                movement.y = 0;
            }
        }
        if (_jumping) {
            float t = Time.time - _lastJumpTime;
            if (t < jumpTime) {
                movement.y += jumpStrength * Time.deltaTime;// * Mathf.Lerp(jumpTime, 0, t);
                Debug.Log("jumping");
            }
            else
                _jumping = false;
        }
        controller.Move(movement * Time.deltaTime);
    }
}
