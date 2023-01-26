using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunProgressionDirector : DescriptionMonoBehavior
{
    public struct RunProgressionData
    {
        public GameSceneSO scene;
        public bool startNewRun;
        public bool transitionOut;
        public bool transitionIn;
    }
        
#if UNITY_EDITOR
    [ColorHeader("Reading - Cold Startup State", ColorHeaderColor.ReadingState)]
    [SerializeField] private ColdStartupDataSO coldStartupState;
#endif

    [ColorHeader("Dependencies", ColorHeaderColor.Dependencies)] 
    [SerializeField] private WorldGenerationProviderSO worldGenerationProvider;
    [SerializeField] private GameplayKeyScenesSO gameplayKeyScenes;

    // Internal state
    private WorldGenerationStrategy worldGenerationStrategyInstance;
    private Queue<RunProgressionData> manualProgressionQueue = new();
    
    // Hard coded progression datas
    private RunProgressionData RunFinishMenuSceneProgressionData => new RunProgressionData()
    {
        scene =  gameplayKeyScenes.RunFinishMenuScene,
        startNewRun = false,
        transitionOut = true,
        transitionIn = true
    };
    
    private RunProgressionData EntrySceneProgressionData => new RunProgressionData()
    {
        scene =  gameplayKeyScenes.EntryScene,
        startNewRun = false,
        transitionOut = true,
        transitionIn = true
    };
    
    private RunProgressionData StartNewRunProgressionData => new RunProgressionData()
    {
        startNewRun = true
    };
    
    /// <summary>
    /// Start the run progression director "state machine", but with loaded data or a cold startup
    /// </summary>
    /// <param name="runSaveData"></param>
    public void LoadstartRunProgressionDirector(RunSaveData runSaveData)
    {
        manualProgressionQueue.Clear();
        
#if UNITY_EDITOR
        if (coldStartupState.isColdStartup)
        {
            manualProgressionQueue.Enqueue(
                new RunProgressionData()
                {
                    scene = coldStartupState.startupScene,
                    startNewRun = false,
                    transitionOut = false,
                    transitionIn = true
                }
            );
            coldStartupState.ConsumeColdStartup();
            // TODO: Tool ways to customize how cold startup interacts with world generation
            worldGenerationStrategyInstance = worldGenerationProvider.CreateWorldGenerationStrategy();
            return;
        }
#endif
        
        if (runSaveData.isRunInProgress)
        {
            // TODO: Set up level generation to re-initialize mid-way through a run
        }
        else
        {
            var toEnqueue = EntrySceneProgressionData;
            toEnqueue.transitionOut = false;
            manualProgressionQueue.Enqueue(toEnqueue);
        }
        
        worldGenerationStrategyInstance = worldGenerationProvider.CreateWorldGenerationStrategy();
    }
    
    /// <summary>
    ///  Restart the run progression director "state machine"
    /// </summary>
    /// <param name="runSaveData"></param>
    public void RestartRunProgressionDirector(RunSaveData runSaveData)
    {
        // Clear the run save data
        runSaveData = new RunSaveData();
        manualProgressionQueue.Clear();
        worldGenerationStrategyInstance = worldGenerationProvider.CreateWorldGenerationStrategy();
        manualProgressionQueue.Enqueue(EntrySceneProgressionData);
    }

    public RunProgressionData GetNextProgressionData()
    {
        if (manualProgressionQueue.Count > 0)
        {
            return manualProgressionQueue.Dequeue();
        }
        else
        {
            var nextLevel = worldGenerationStrategyInstance.GetNextLevel();

            if (nextLevel == null)
            {
                manualProgressionQueue.Enqueue(StartNewRunProgressionData);
                return RunFinishMenuSceneProgressionData;
            }
            else
            {
                return new RunProgressionData()
                {
                    scene = nextLevel,
                    startNewRun = false,
                    transitionOut = true,
                    transitionIn = true
                };
            }
        }
    }
}
