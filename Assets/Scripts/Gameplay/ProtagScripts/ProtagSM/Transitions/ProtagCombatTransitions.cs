using System;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public partial class ProtagTransitions : TransitionTable<ProtagBlackboard>
{
    #region Combat Selector

    public bool ToCombatSelector()
    {
        var buffer = context.inputProvider.gameplayInputBuffer;
        bool validAction = false;
        // Iterate through most recent `Combat Commands` in input buffer
        //   Until a valid action can be performed
        while (!validAction && buffer.PeekFirstByFlags(
                   (int)PlayerUserInputProvider.PlayerCommandFlag.CombatCommand,
                   out LinkedListNode<GameplayInputBuffer.GameplayCommand> command))
        {

            var commandStruct = command.Value;
            // Primary attack found
            if (commandStruct.commandID == (int)PlayerUserInputProvider.PlayerCommandID.PrimaryAttackCommandDown)
            {
                TransitionTo<ProtagArmedAttack>();
                validAction = true;
            }
            // Secondary attack found
            else if (commandStruct.commandID == (int)PlayerUserInputProvider.PlayerCommandID.SecondaryAttackCommandDown)
            {
                TransitionTo<ProtagThrowAttack>();
                validAction = true;
            }
            
            // Pop the combat command, despite being valid or not
            buffer.PopCommand(command);
        }

        // When true, found a valid combat actions
        return validAction;
    }

    #endregion
}
