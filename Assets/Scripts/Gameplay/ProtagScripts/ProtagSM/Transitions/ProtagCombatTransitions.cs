using System;
using System.Collections.Generic;
using SimpleStateMachine;
using UnityEngine;

public class ProtagCombatTransitions : TransitionTable<ProtagBlackboard>
{

    #region Combat Selector

    public bool ToCombatSelector(ref State<ProtagBlackboard> c)
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
                c = GetState<ProtagArmedAttack>();
                validAction = true;
            }
            else if (commandStruct.commandID == (int)PlayerUserInputProvider.PlayerCommandID.SecondaryAttackCommandDown)
            {
                c = GetState<ProtagThrowAttack>();
                validAction = true;
            }
            
            buffer.PopCommand(command);
        }

        return validAction;
    }

    #endregion
}
