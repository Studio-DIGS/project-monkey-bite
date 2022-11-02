using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputSource<InputState>
{
    public abstract InputState ProcessInputState(InputState data);
}
