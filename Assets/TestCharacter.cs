using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : BasicCharacter {
    // Member variables
    // ----------------------------------------------------------------------------
    // protected component references
    public PInputStateProviderSO inputProvider;

    // public constants
    public const float gravity = -10f;

    // public inspector fields
    public float moveSpeed = 10f;
    public float acceleration = 5f;

    // Update is called once per frame
    void Update() {
        PlayerInputState state = inputProvider.GetInputState();
        movement.x = state.horizontalAxis * moveSpeed;

        //movement.x = input.x * moveSpeed;
        //// if (Input.GetKeyDown("space"))
        //// {
        ////     movement.y += 
        //// }
        movement.y += gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);
    }
}
