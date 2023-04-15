using System.Collections;
using System.Collections.Generic;
using MushiCore;
using MushiCore.EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(menuName="Architecture/SceneManagement/SceneStateSO")]
public class CurrentSceneStateSO : DescriptionBaseSO
{
    [EditorReadOnly] public bool canStartNewSceneOperation;
}
