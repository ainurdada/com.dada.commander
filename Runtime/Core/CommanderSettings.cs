using UnityEngine;

namespace Dada.Commander.Core
{
    public static class CommanderSettings
    {
        #region Colors
        public static string LogErrorColorHtml
        {
            get
            {
                return CommanderManager.Instance.errorColor;
            }
            set
            {
                CommanderManager.Instance.errorColor = value;
            }
        }
        public static Color LogErrorColor
        {
            get
            {
                if (ColorUtility.TryParseHtmlString(LogErrorColorHtml, out Color color))
                {
                    return color;
                }
                else
                {
                    return Color.green;
                }
            }
            set
            {
                LogErrorColorHtml = $"#{ColorUtility.ToHtmlStringRGB(value)}";
            }
        }
        public static string LogColorHtml
        {
            get
            {
                return CommanderManager.Instance.textColor;
            }
            set
            {
                CommanderManager.Instance.textColor = value;
            }
        }
        public static Color LogColor
        {
            get
            {
                if (ColorUtility.TryParseHtmlString(LogColorHtml, out Color color))
                {
                    return color;
                }
                else
                {
                    return Color.white;
                }
            }
            set
            {
                LogColorHtml = $"#{ColorUtility.ToHtmlStringRGB(value)}";
            }
        }
        public static string LogWarningColorHtml
        {
            get
            {
                return CommanderManager.Instance.warningColor;
            }
            set
            {
                CommanderManager.Instance.warningColor = value;
            }
        }
        public static Color LogWarningColor
        {
            get
            {
                if (ColorUtility.TryParseHtmlString(LogWarningColorHtml, out Color color))
                {
                    return color;
                }
                else
                {
                    return Color.yellow;
                }
            }
            set
            {
                LogWarningColorHtml = $"#{ColorUtility.ToHtmlStringRGB(value)}";
            }
        }
        #endregion

        public static bool DuplicateLog
        {
            get
            {
                return Commander.duplicateLog;
            }
            set
            {
                Commander.duplicateLog = value;
            }
        }
    }
}
