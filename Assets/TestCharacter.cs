using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PathController))]
public class TestCharacter : MonoBehaviour {
    // State enums
    // ----------------------------------------------------------------------------
    public enum STATE {
        Idle,
        Walking,
        Jumping,
        Falling
    }

    // Member variables
    // ----------------------------------------------------------------------------
    // component references
    PathController controller;
    public PlayerUserInputProvider inputProvider;

    // public constants

    // public inspector fields
    public float moveSpeed = 10f;
    public float acceleration = 5f;
    public float gravity = -10f;
    public float jumpStrength = 20f;
    public float minJumpTime = 0.2f;
    public float maxJumpTime = 1f;

    // private members
    private bool _jumping;
    private float _timeOfStateChange;
    private Vector2 _movement;
    private Vector2 _lastPosition;
    private PlayerInputState _inputData;

    // read-only properties
    private STATE _state;
    public STATE State => _state;

    void Start()
    {
        controller = GetComponent<PathController>();
        ChangeState(STATE.Idle, false);
    }

    // Update is called once per frame
    void Update() {
        inputProvider.GetInputState(ref _inputData);

        PerformStateEffects(true);

        controller.Move(_movement * Time.deltaTime);
        
        //_movement.x = state.horizontalAxis * moveSpeed;
        //_movement.y += gravity * Time.deltaTime;

        //if (controller.OnGround()) {
        //    Debug.Log("GROUND");
        //    _movement.y = Mathf.Max(_movement.y, -1f);
        //    if (state.jumpHeld) {
        //        _jumping = true;
        //        _lastJumpTime = Time.time;
        //        _movement.y = 0;
        //    }
        //}
        //if (_jumping) {
        //    float t = Time.time - _lastJumpTime;
        //    if (t < jumpTime) {
        //        _movement.y += jumpStrength * Time.deltaTime;// * Mathf.Lerp(jumpTime, 0, t);
        //        Debug.Log("jumping");
        //    }
        //    else
        //        _jumping = false;
        //}
        //controller.Move(_movement * Time.deltaTime);
    }

    // State machine functions
    void ChangeState(STATE newState, bool performState = true) {
        _state = newState;
        _timeOfStateChange = Time.time;
        if (performState) { 
            PerformStateEffects(false);
        }
    }

    void PerformStateEffects(bool checkTransitions = false) {
        switch (_state) {
            case STATE.Idle:
                StateIdle(checkTransitions);
                break;
            case STATE.Walking:
                StateWalking(checkTransitions);
                break;
            case STATE.Jumping:
                StateJumping(checkTransitions);
                break;
            case STATE.Falling:
                StateFalling(checkTransitions);
                break;
        }
    }

    // State effects
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
    }
}
