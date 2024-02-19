using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that need a public description field.
/// </summary>
public class DescriptionBaseSO : ScriptableObject
{
#if UNITY_EDITOR
	[TextArea] public string description;
#endif	
}
