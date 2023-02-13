using System.Collections.Generic;
using System.Text;
using MushiCore.Editor;
using UnityEditor;

namespace MushiSOArchitecture.Editor
{
    public class ChannelTemplate
    {
        private static string path = "Assets/MushiStuff/MushiSOArchitecture/Channels/Editor";

        [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Event Channel Script", false, 0)]
        public static void CreateEventChannelScript()
        {
            AssetTemplateUtility.CreateNamedAssetFromTemplate(
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
            AssetTemplateUtility.CreateNamedAssetFromTemplate(
                $"{path}/FuncChannelScriptTemplate.txt",
                "ChannelSO.cs",
                "#FILENAME#",
                "#RAWFILENAME#",
                "FuncChannelSO"
            );
        }

        private const int eventArgs = 8;
        private const int funcArgs = 8;
        private static string genericChannelsPath = "Assets/MushiStuff/MushiSOArchitecture/Channels";

        [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Generate Base Generic Channels Script", false, 0)]
        public static void CreateGenericChannelScript()
        {
            var processors = new AssetTemplateUtility.TemplateStringProcessor[] { new GenericChannelsGenerator() };
            AssetTemplateUtility.CreateAssetFromTemplate(
                $"{path}/GenericChannelsScriptTemplate.txt",
                $"{genericChannelsPath}/GenericChannelsSO.cs",
                contentProcessors: processors
            );
        }

        private class GenericChannelsGenerator : AssetTemplateUtility.TemplateStringProcessor
        {
            public StringBuilder ProcessString(StringBuilder input)
            {
                string raw = input.ToString();

                Dictionary<string, string> subBlocks = ScriptTemplateUtils.GetSubBlocks(raw);
                string mainBlock = ScriptTemplateUtils.GetMainBlock(raw);

                StringBuilder buildGenerics = new StringBuilder();

                buildGenerics.Append(mainBlock);

                void AddLine(string str)
                {
                    buildGenerics.Append(str + "\n");
                }

                var genericParams = new StringBuilder();
                var raiseParams = new StringBuilder();
                var invokeParams = new StringBuilder();

                // Events
                void CreateGenericEvent(string generic, string raise, string invoke)
                {
                    var body = new StringBuilder(subBlocks["EVENTBODY"]);
                    body.Replace("#PARAMS#", generic);
                    body.Replace("#RAISE_PARAMS#", raise);
                    body.Replace("#INVOKE_PARAMS#", invoke);
                    AddLine(body.ToString());
                }

                CreateGenericEvent("", "", "");

                genericParams.Append("<");

                for (int i = 1; i <= eventArgs; i++)
                {
                    genericParams.Append($"T{i},");
                    raiseParams.Append($"T{i} arg{i},");
                    invokeParams.Append($"arg{i},");

                    CreateGenericEvent(
                        genericParams.ToString().TrimEnd(',') + ">",
                        raiseParams.ToString().TrimEnd(','),
                        invokeParams.ToString().TrimEnd(','));
                }

                // Funcs
                genericParams.Clear();
                raiseParams.Clear();
                invokeParams.Clear();

                void CreateGenericFunc(string generic, string raise, string invoke)
                {
                    var body = new StringBuilder(subBlocks["FUNCBODY"]);
                    body.Replace("#PARAMS#", generic);
                    body.Replace("#RAISE_PARAMS#", raise);
                    body.Replace("#INVOKE_PARAMS#", invoke);
                    AddLine(body.ToString());
                }

                CreateGenericFunc("", "", "");

                for (int i = 1; i <= eventArgs; i++)
                {
                    genericParams.Append($"T{i},");
                    raiseParams.Append($"T{i} t{i},");
                    invokeParams.Append($"t{i},");

                    CreateGenericFunc(
                        genericParams.ToString(),
                        raiseParams.ToString().TrimEnd(','),
                        invokeParams.ToString().TrimEnd(','));
                }

                input.Clear();
                input.Append(buildGenerics);
                return buildGenerics;
            }
        }
    }
}