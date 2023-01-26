using MushiEditorTools.AssetCreationUtils;
using UnityEditor;
using UnityEngine;

public class ChannelTemplate
{
    private static string path = "Assets/MushiStuff/MushiSOArchitecture/Channels/Editor";

    [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Event Channel Script", false, 0)]
    public static void CreateEventChannelScript()
    {
        AssetTemplateUtility.CreateScriptFromTemplate(
            $"{path}/EventChannelScriptTemplate.txt",
            "EventChannelSO.cs",
            "#FILENAME#",
            "#RAWFILENAME#",
            "EventChannelSO"
        );
    }
    
    [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Func Channel Script", false, 0)]
    public static void CreateFuncChannelScript()
    {
        AssetTemplateUtility.CreateScriptFromTemplate(
            $"{path}/FuncChannelScriptTemplate.txt",
            "ChannelSO.cs",
            "#FILENAME#",
            "#RAWFILENAME#",
            "FuncChannelSO"
        );
    }
}
