using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayInputBuffer 
{
    public struct GameplayCommand
    {
        public readonly int commandID;
        public readonly object commandValue;
        public readonly int flags;
        public readonly float expireTime;

        public GameplayCommand(int commandID, object commandValue, float expireTime, int flags)
        {
            this.commandID = commandID;
            this.commandValue = commandValue;
            this.expireTime = expireTime;
            this.flags = flags;
        }

        public T ReadValue<T>()
        {
            return (T)commandValue;
        }
    }
    
    public LinkedList<GameplayCommand> actionCommandBuffer = new();

    /// <summary>
    /// Add a new command to the gameplay command buffer
    /// </summary>
    /// <param name="commandID"></param>
    /// <param name="commandValue"></param>
    /// <param name="expireTime"></param>
    /// <param name="flags"></param>
    public void BufferGameplayCommand(int commandID, object commandValue, float lifetime, int flags = 0)
    {
        var command = new GameplayCommand(commandID, commandValue, Time.unscaledTime + lifetime, flags);
        
        var node = actionCommandBuffer.First;
        while (node != null && node.Value.expireTime < command.expireTime)
        {
            node = node.Next;
        }
        
        if (node == null)
        {
            actionCommandBuffer.AddLast(command);
            return;
        }

        actionCommandBuffer.AddBefore(node, command);
    }
    
    public void PopCommand(LinkedListNode<GameplayCommand> command)
    {
        actionCommandBuffer.Remove(command);
    }

    public bool PeekFirst(out LinkedListNode<GameplayCommand> command)
    {
        return FindFirstCommand(
            (a) => true, 
            out command);
    }
    
    public bool PeekFirstByID(int commandID, out LinkedListNode<GameplayCommand> command)
    {
        return FindFirstCommand(
            (a) => a.Value.commandID == commandID, 
            out command);
    }
    
    public bool PeekFirstByFlags(int flags, out LinkedListNode<GameplayCommand> command)
    {
        return FindFirstCommand(
            (a) => (a.Value.flags | flags) == a.Value.flags, 
            out command);
    }
    
    public bool PeekFirstByIDAndFlags(int commandID, int flags, out LinkedListNode<GameplayCommand> command)
    {
        return FindFirstCommand(
            (a) => 
                (a.Value.commandID == commandID) 
                && (a.Value.flags | flags) == a.Value.flags, 
            out command);
    }

    /// <summary>
    /// Find the first command in the buffer that satisfies some requirement
    /// </summary>
    /// <param name="requirement"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    private bool FindFirstCommand(
        Func<LinkedListNode<GameplayCommand>, bool> requirement, 
        out LinkedListNode<GameplayCommand> command)
    {
        var node = actionCommandBuffer.First;
        
        while (node != null)
        {
            if (requirement(node))
            {
                command = node;
                return true;
            }
            node = node.Next;
        }

        command = null;
        return false;
    }

    /// <summary>
    /// Starting from the front, remove the commands that have expired
    /// This must be externally called when needed. Make sure to ensure that
    /// commands last for at least one frame.
    /// </summary>
    public void RemoveExpired()
    {
        var node = actionCommandBuffer.First;
        while (node != null && node.Value.expireTime < Time.unscaledTime)
        {
            actionCommandBuffer.RemoveFirst();
            node = actionCommandBuffer.First;
        }
    }
    
}

