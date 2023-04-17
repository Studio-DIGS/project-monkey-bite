using SimpleStateMachine;
using UnityEngine;

public class ProtagArmedAttack : ProtagState
{
    public override void EnterState()
    {
        animationController.Play("Attack1");
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState(float deltaTime)
    {
        if (stateMachine.CurrentStateDuration >= animationController.GetCurrentAnimatorStateInfo(0).length)
        {
            transitions.ToProtagStateSelector();
        }
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        // Stop the player
        Vector2 groundNormal = controllerMotor.CurrentGroundState.GroundNormal;
        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            0, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            fixedDeltaTime, 
            groundNormal);
    }
}
