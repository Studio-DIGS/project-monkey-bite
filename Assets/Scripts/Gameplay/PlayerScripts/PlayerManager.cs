using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PathController))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerBlackboard blackboard;
    
    // Private fields
    private PathController controller;
    private PlayerMovementStateMachine movementStateMachine;

    void Start()
    {
        controller = GetComponent<PathController>();
        movementStateMachine = new PlayerMovementStateMachine(blackboard);
        movementStateMachine.InitializeEntryState<PlayerIdleState>();
    }
    
    void Update() {
        blackboard.UpdateInputState();

        movementStateMachine.Update();

        //controller.Move(_movement * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        movementStateMachine.FixedUpdate();
    }

    /*// State effects
    void StateIdle(bool checkTransitions = false) {
        // Transitions
        if (checkTransitions) {
            if (BasicStateTransitions())
                return;
        }
        //Debug.Log("STATE = Idle");

        // Effects
        _movement.x = 0;
        _movement.y = 0;
    }
    void StateWalking(bool checkTransitions = false) { 
        // Transitions
        if (checkTransitions) {
            if (BasicStateTransitions())
                return;
        }
        //Debug.Log("STATE = Walking");

        // Effects
        _movement.x = _inputData.horizontalAxis * moveSpeed;
        _movement.y = 0;        
    }
    void StateJumping(bool checkTransitions = false) { 
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
    }
    void StateFalling(bool checkTransitions = false) { 
        // Transitions
        if (checkTransitions) {
            if (BasicStateTransitions())
                return;
        }
        //Debug.Log("STATE = Falling");

        // Effects
        _movement.x = _inputData.horizontalAxis * moveSpeed;
        _movement.y += gravity * Time.deltaTime;
    }

    // State transitions
    bool BasicStateTransitions() {
        if (controller.OnGround()) { 
            if (_state != STATE.Jumping && _inputData.jumpHeld) {
                ChangeState(STATE.Jumping);
                return true;
            }
            else if(_state != STATE.Walking && _inputData.horizontalAxis != 0) {
                ChangeState(STATE.Walking);
                return true;
            }
            else if(_state != STATE.Idle && _movement == Vector2.zero) {
                ChangeState(STATE.Idle);
                return true;
            }
        }
        else {
            if (_state != STATE.Falling && _state != STATE.Jumping) {
                ChangeState(STATE.Falling);
                return true;
            }
        }
        return false;
    }*/
}
