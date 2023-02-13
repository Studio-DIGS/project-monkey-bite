using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MushiCore.Editor
{
    /// <summary>
    /// Some Utils for dealing with script templates. Used with <see cref="AssetTemplateUtility"/>
    /// </summary>
    public class ScriptTemplateUtils
    {
        public static Dictionary<string, string> GetSubBlocks(string str)
        {
            var res = new Dictionary<string, string>();
            Regex regexSubBlock = new Regex(@"<#.*?>((\n|.)*?)<#>");
            var matches = regexSubBlock.Matches(str);

            foreach (Match match in matches)
            {
                string raw = match.ToString();
                string name = raw.Split('>')[0].Substring(2);

                int headLength = name.Length + 3;

                string content = raw
                    .Substring(headLength + 1, raw.Length - headLength - 4);

                res.TryAdd(name, content);
            }

            return res;
        }

        public static string GetMainBlock(string str)
        {
            Regex regexMain = new Regex(@"<=>((\n|.)*?)<=>");
            var match = regexMain.Match(str);

            string res = match.ToString().Trim(
                '<', '>', '='
            );

            return res;
        }
    }
}