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
        GroundStop(fixedDeltaTime);
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
        
        return
            // If animationFinished and next player input is also an attack, continue combo
                animationFinished && transitions.ToCombatSelector() && (stateMachine.PreviousState == this || EndCombo()) 
            // Or, if full attack time completed, end combo and transition out
               || maxTimePassed && EndCombo() && transitions.ToMovementSelector();
    }

    private bool EndCombo()
    {
        comboCounter = 0;
        return true;
    }
}
