using System;
using UnityEngine;
using UnityEngine.Splines;

public class LevelPathManager : MonoBehaviour
{
    [SerializeField] private GameplayLevelStateSO currentGameplayLevelBlackboard;
    [SerializeField] private SplineContainer levelCurve;

    private void OnEnable()
    {
        currentGameplayLevelBlackboard.levelPath = levelCurve;
    }
}
