using System;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Architecture/SaveSystem/SavePathConfig")]
public class RelativeSavePathConfigSO : ScriptableObject
{
    [SerializeField] private string profileIdentifierSymbol;
    [SerializeField] private string relativeDirectory;

    public string RelativeDirectory => relativeDirectory;
    
    [SerializeField] private string fileTypePostfix;

    public string FileTypePostfix => fileTypePostfix;
    
    [SerializeField] private string saveFilename;

    [ReadOnly, SerializeField] private string finalPath;

    public string GetProfileSaveRelativePath(string profileIdentifier)
    {
        return finalPath.Replace(profileIdentifierSymbol, profileIdentifier);
    }

    private void BuildRelativePath()
    {
        StringBuilder sb = new StringBuilder();
        if (relativeDirectory != String.Empty)
        {
            sb.Append(relativeDirectory.Trim('/'));
            sb.Append('/');
        }
        sb.Append(saveFilename);
        if (fileTypePostfix != String.Empty)
        {
            sb.Append(".");
            sb.Append(fileTypePostfix.Trim('.'));
        }
        finalPath = sb.ToString();
    }

    private void OnValidate()
    {
        BuildRelativePath();
    }
}
