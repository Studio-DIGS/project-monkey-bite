using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/Input/InputSources/UIInputSource")]
public class UIInputSource : ScriptableObject, InputSource<PlayerInputState>
{
    public bool isInUI;
    public PlayerInputState ProcessInputState(PlayerInputState data)
    {
        if (isInUI)
            return new PlayerInputState();
        return data;
    }
}
