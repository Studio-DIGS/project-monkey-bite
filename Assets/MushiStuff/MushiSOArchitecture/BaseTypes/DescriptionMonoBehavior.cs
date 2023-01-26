using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionMonoBehavior : MonoBehaviour
{
#if UNITY_EDITOR
    [TextArea] public string description;
#endif	
}
