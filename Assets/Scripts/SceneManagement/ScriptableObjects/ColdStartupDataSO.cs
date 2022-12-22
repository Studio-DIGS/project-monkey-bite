using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SceneManagement/ColdStartupDataSO")]
public class ColdStartupDataSO : DescriptionBaseSO
{
    [DoNotSerialize] public GameSceneSO startupScene;
    [DoNotSerialize] public bool isColdStartup;

    /// <summary>
    /// Clears cold startup. Manager-level scripts should consume the cold startup once it is finished loading.
    /// This is done to prevent cold startups from occuring multiple times in a single play session
    /// </summary>
    public void ConsumeColdStartup()
    {
        startupScene = null;
        isColdStartup = false;
    }
}
