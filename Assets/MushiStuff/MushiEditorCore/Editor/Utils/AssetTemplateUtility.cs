using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace MushiEditorTools.AssetCreationUtils
{
    /// <summary>
    /// Utility functions for creating assets and scripts from existing templates
    /// </summary>
    public class AssetTemplateUtility
    {
        public delegate void OnCreatedFromTemplateAction(string newFilePath);
        
        /// <summary>
        /// Create a new script using a template text file.
        /// </summary>
        /// <param name="templatePath">path to template text file</param>
        /// <param name="defaultFileName"></param>
        /// <param name="nameSymbol">pattern to replace with the script name (includes any specified postfix/prefix)</param>
        /// <param name="rawNameSymbol">pattern to replace with the script name (excludes postfix/prefix)</param>
        /// <param name="appendPostfix">postfix to append to end of name (if user provided name does not contain)</param>
        /// <param name="appendPrefix">prefix to append to end of name (if user provided name does not contain)</param>
        public static OnCreatedFromTemplateAction CreateScriptFromTemplate(
            string templatePath, 
            string defaultFileName, 
            string nameSymbol = default,
            string rawNameSymbol = "", 
            string appendPostfix = "", 
            string appendPrefix = "")
        {
            // Set up end file naming action 
            var endAction = ScriptableObject.CreateInstance<CreateFileTemplateAction>();
            endAction.nameSymbol = nameSymbol;
            endAction.rawNameSymbol = rawNameSymbol;
            endAction.appendPostFix = appendPostfix;
            endAction.appendPreFix = appendPrefix;
            
            // Begin name editing (this also creates the new file)
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                endAction,
                defaultFileName,
                (Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
                templatePath);
            
            return endAction.onCreated;
        }
        
        /// <summary>
        /// Copies from a template with no text replacement of any sort
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="defaultFileName"></param>
        public static OnCreatedFromTemplateAction CreateAssetFileFromTemplate(
            string templatePath, 
            string defaultFileName,
            OnCreatedFromTemplateAction action)
        {
            // Set up end file naming action 
            var endAction = ScriptableObject.CreateInstance<CreateFileTemplateAction>();

            // Begin name editing (this also creates the new file)
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                endAction,
                defaultFileName,
                (Texture2D)EditorGUIUtility.IconContent("d_SceneAsset Icon").image,
                templatePath);

            endAction.onCreated += action;
            return endAction.onCreated;
        }

        private class CreateFileTemplateAction : EndNameEditAction
        {
            public string nameSymbol = "";
            public string rawNameSymbol = "";
            public string appendPostFix = "";
            public string appendPreFix = "";

            /// <summary>
            /// Invoked when the end name edit action finishes. Contains the relative path of the new asset
            /// </summary>
            public OnCreatedFromTemplateAction onCreated;
            
            /// <summary>
            /// Called when the file name editing finishes
            /// </summary>
            /// <param name="instanceId"></param>
            /// <param name="pathName"></param>
            /// <param name="resourceFile"></param>
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                // Text from template file
                string text = File.ReadAllText(resourceFile);

                // Name formatting
                string rawFileName = Path.GetFileName(pathName).Trim();
                string newFileName = rawFileName;
                    
                // Append specified postfix (before extension)
                if (appendPostFix != "")
                {
                    var splitFileName = newFileName.Split(".");
                    if (!splitFileName[0].EndsWith(appendPostFix))
                    {
                        newFileName = newFileName.Replace(".", appendPostFix + ".");
                    }
                }

                if (appendPreFix != "")
                {
                    // Append specified prefix
                    if (!newFileName.StartsWith(appendPreFix))
                    {
                        newFileName = appendPreFix + newFileName;
                    }
                }

                // Update path name
                pathName = pathName.Replace(rawFileName, newFileName);

                // Swap out replacement symbol for formatted script name
                if (nameSymbol != "")
                {
                    string fileNameWithoutExtension = newFileName.Split(".")[0];
                    text = text.Replace(nameSymbol, fileNameWithoutExtension);
                }
                
                // Swap out raw replacement symbol for raw script name (with prefix/postfix trimmed)
                if (rawNameSymbol != "")
                {
                    string fileNameWithoutExtension = rawFileName.Split(".")[0];
                    
                    // Trim out prefix/postfix
                    fileNameWithoutExtension = TrimStart(fileNameWithoutExtension, appendPreFix);
                    fileNameWithoutExtension = TrimEnd(fileNameWithoutExtension, appendPostFix);
                    
                    text = text.Replace(rawNameSymbol, fileNameWithoutExtension);
                }

                // Write processed text into the file
                string fullPath = Path.GetFullPath(pathName);
                var encoding = new UTF8Encoding(true);
                File.WriteAllText(fullPath, text, encoding);
                AssetDatabase.ImportAsset(pathName);
                ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object)));
                
                onCreated?.Invoke(pathName);
            }

            private string TrimEnd(string toTrim, string trimString)
            {
                if (toTrim.EndsWith(trimString))
                {
                    return toTrim.Substring(0, toTrim.Length - trimString.Length);
                }

                return toTrim;
            }
            
            private string TrimStart(string toTrim, string trimString)
            {
                if (toTrim.StartsWith(trimString))
                {
                    return toTrim.Substring(trimString.Length, toTrim.Length - trimString.Length);
                }

                return toTrim;
            }

            public override void Cancelled(int instanceId, string pathName, string resourceFile)
            {
                base.Cancelled(instanceId, pathName, resourceFile);
            }
        }
        
    }
}

