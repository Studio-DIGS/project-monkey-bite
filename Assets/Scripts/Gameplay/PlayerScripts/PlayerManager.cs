using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : DescriptionMonoBehavior
{
    [ColorHeader("Dependencies")]
    [SerializeField] private PlayerBlackboard blackboard;

    // Fields
    private PlayerMovementStateMachine movementStateMachine;

    void Start()
    {
        movementStateMachine = new PlayerMovementStateMachine(blackboard);
        movementStateMachine.InitializeEntryState<PlayerIdleState>();
    }
    
    void Update() 
    {
        blackboard.UpdateInputState();
        movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        blackboard.movementContextController.UpdateContext();
        movementStateMachine.FixedUpdate();
    }

    /*void StateJumping(bool checkTransitions = false) { 
        // Transitions
        float t = Time.time - _timeOfStateChange;
        if (checkTransitions) {
            if (t >= minJumpTime) {
                if (controller.OnGround()) {
                    ChangeState(STATE.Idle, true);
                    return;
                }
                if ((!_inputData.jumpHeld || t >= maxJumpTime)) {
                    ChangeState(STATE.Falling);
                    StateFalling();
                    return;
                }
            }
        }
        //Debug.Log("STATE = Jumping");

        // Effects
        _movement.x = _inputData.horizontalAxis * moveSpeed;
        _movement.y = jumpStrength * (maxJumpTime - t);
    }*/
}
