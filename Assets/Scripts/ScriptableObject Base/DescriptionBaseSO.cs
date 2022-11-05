using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that need a public description field.
/// </summary>
public class DescriptionBaseSO : SerializableScriptableObject
{
#if UNITY_EDITOR
	[TextArea] public string description;
#endif	
}
