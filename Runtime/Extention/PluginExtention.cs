using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dada.Commander
{
    internal static class PluginExtention
    {
        #region string
        public static string SetColor(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
        public static string SetColor(this string text, string colorHTML)
        {
            return $"<color={colorHTML}>{text}</color>";
        }
        public static string[] SplitLeavingInsideContext(this string text, char separator, char from, char to)
        {
            var reg = new Regex($"(?<open>\\{from}).*?(?<final-open>\\{to})");
            var matches = reg.Matches(text).Cast<Match>().
                Select(m => m.Groups["final"].Value).ToArray();
            List<string> result = new();
            foreach (var match in matches)
            {
                int index = text.IndexOf(match);
                var behindMatch = text.Substring(0, index);
                if (index > 0)
                {
                    behindMatch = behindMatch.Replace($"{from}", "").Replace($"{to}", "");
                    result.AddRange(behindMatch.Split(separator, StringSplitOptions.RemoveEmptyEntries));
                }
                result.Add(match);
                text = text.Remove(0, index + match.Length + 1);
            }
            if (text.Length > 0)
            {
                result.AddRange(text.Split(separator, StringSplitOptions.RemoveEmptyEntries));
            }
            return result.ToArray();
        }
        #endregion
    }
}
