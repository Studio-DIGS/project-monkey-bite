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
        while (!validAction && buffer.PeekFirstByFlags(
                   (int)PlayerUserInputProvider.PlayerCommandFlag.CombatCommand,
                   out LinkedListNode<GameplayInputBuffer.GameplayCommand> command))
        {

            var commandStruct = command.Value;
            if (commandStruct.commandID == (int)PlayerUserInputProvider.PlayerCommandID.PrimaryAttackCommandDown)
            {
                TransitionTo<ProtagArmedAttack>();
                validAction = true;
            }
            else if (commandStruct.commandID == (int)PlayerUserInputProvider.PlayerCommandID.SecondaryAttackCommandDown)
            {
                TransitionTo<ProtagThrowAttack>();
                validAction = true;
            }
            
            buffer.PopCommand(command);
        }

        return validAction;
    }

    #endregion
}
