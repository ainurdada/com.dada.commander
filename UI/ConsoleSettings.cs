using Dada.Commander.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Commander.Ui
{
    public class ConsoleSettings : MonoBehaviour
    {
        #region Instance
        static ConsoleSettings instance;
        public static ConsoleSettings Instance { get { return instance; } }
        #endregion

        [SerializeField] bool hideAtTheStart;

        [Header("Background")]
        [SerializeField] Color logBackgroundColor;
        [SerializeField] Color inputBackgroundColor;

        [Header("Log settings")]
        [SerializeField] CommandLog commandLog;
        [SerializeField] LogResult logResult;

        #region Properties
        public bool HideAtTheStart
        {
            get { return hideAtTheStart; }
            set
            {
                hideAtTheStart = value;
                ConsoleUI.Instance.hideAtTheBeginning = hideAtTheStart;
            }
        }
        public Color LogBackgroundColor
        {
            get { return logBackgroundColor; }
            set
            {
                logBackgroundColor = value;
                ConsoleUI.Instance.logBackground.color = logBackgroundColor;
            }
        }
        public Color InputBackgroundColor
        {
            get { return inputBackgroundColor; }
            set
            {
                inputBackgroundColor = value;
                ConsoleUI.Instance.inputBackGround.color = inputBackgroundColor;
            }
        }
        public Color CommandColor
        {
            get { return commandLog.commandColor; }
            set
            {
                commandLog.commandColor = value;
                ConsoleUI.Instance.cmdColor = $"#{ColorUtility.ToHtmlStringRGB(commandLog.commandColor)}";
            }
        }
        public string CommandPrefix
        {
            get { return commandLog.commandPrefix; }
            set
            {
                commandLog.commandPrefix = value;
                ConsoleUI.Instance.commandPrefix = commandLog.commandPrefix;
            }
        }
        public Color LogColor
        {
            get { return logResult.logColor; }
            set
            {
                logResult.logColor = value;
                CommanderSettings.LogTextColor = $"#{ColorUtility.ToHtmlStringRGB(logResult.logColor)}";
            }
        }
        public Color LogErrorColor
        {
            get { return logResult.logErrorColor; }
            set
            {
                logResult.logErrorColor = value;
                CommanderSettings.LogErrorColor = $"#{ColorUtility.ToHtmlStringRGB(logResult.logErrorColor)}";
            }
        }
        public Color PrefixColor
        {
            get { return commandLog.prefixColor; }
            set
            {
                commandLog.prefixColor = value;
                ConsoleUI.Instance.prefixColor = $"#{ColorUtility.ToHtmlStringRGB(commandLog.prefixColor)}";
            }
        }
        #endregion

        private void Start()
        {
            instance = this;

            HideAtTheStart = hideAtTheStart;

            LogBackgroundColor = logBackgroundColor;
            InputBackgroundColor = inputBackgroundColor;
            CommandColor = commandLog.commandColor;
            CommandPrefix = commandLog.commandPrefix;
            LogColor = logResult.logColor;
            LogErrorColor = logResult.logErrorColor;
            PrefixColor = commandLog.prefixColor;
        }

        [Serializable]
        class CommandLog
        {
            public Color commandColor;
            public string commandPrefix;
            public Color prefixColor;
        }

        [Serializable]
        class LogResult
        {
            public Color logColor;
            public Color logErrorColor;
        }
    }
}
