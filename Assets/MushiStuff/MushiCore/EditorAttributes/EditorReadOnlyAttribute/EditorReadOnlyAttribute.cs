using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class EditorReadOnlyAttribute : PropertyAttribute
{
}