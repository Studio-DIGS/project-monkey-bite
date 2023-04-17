using System.Collections;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagArmedAttack : ProtagState
{
    private int comboCounter;

    public override void EnterState()
    {
        Attack();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState(float deltaTime)
    {
        TryTransitionOut();
    }

    public override void FixedUpdateState(float fixedDeltaTime)
    {
        // Stop the player
        // TODO: Make it so player can't turn around during attack
        Vector2 groundNormal = controllerMotor.CurrentGroundState.GroundNormal;
        playerSimplePathMovement.SimpleGroundedHorizontalMovement(
            0, 
            hMoveProfile.groundedWalkVel,
            hMoveProfile.groundedWalkAccel,
            hMoveProfile.groundedFriction,
            fixedDeltaTime, 
            groundNormal);
    }

    private void Attack()
    {
        // Debug.Log("Initiate " + combatProfile.combo[comboCounter]);
        if (comboCounter <= combatProfile.combo.Count)
        {
            // cycle to next attack animation
            animationController.runtimeAnimatorController = combatProfile.combo[comboCounter].animatorOV;
            animationController.Play("Attack", 0, 0);
            comboCounter++; // increment combo to next attack

            // Reset combo index to 0 if it exceeds array size
            if (comboCounter >= combatProfile.combo.Count)
            {
                comboCounter = 0;
            }
        }

    }

    private bool TryTransitionOut()
    {
        bool animationFinished = animationController.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && animationController.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        bool maxTimePassed = stateMachine.CurrentStateDuration > animationController.GetCurrentAnimatorStateInfo(0).length + 0.2f; 
        
        // If animationFinished and player inputting attack, cycle back to attack state
        // Or if no inputting attack, end combo and go back to idle
        return animationFinished && transitions.ToCombatSelector()
                || maxTimePassed && EndCombo() && transitions.ToMovementSelector();
    }

    private bool EndCombo()
    {
        comboCounter = 0;
        return true;
    }
}
