using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dada.Commander
{
    internal static class PluginExtention
    {
        public static string SetColor(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
        public static string SetColor(this string text, string colorHTML)
        {
            return $"<color={colorHTML}>{text}</color>";
        }
    }
}
