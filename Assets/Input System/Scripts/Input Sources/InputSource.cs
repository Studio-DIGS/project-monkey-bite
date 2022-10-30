using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputSource<T>
{
    public abstract T ProcessInputState(T data);
}
