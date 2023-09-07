using System;
using System.Collections.Generic;
using Dada.Commander.Core;
using UnityEngine;

namespace Dada.Commander
{
    public static class Commander
    {
        internal static bool duplicateLog = false;

        /// <summary>
        /// List of previous commands, that have been sent to Commander
        /// </summary>
        public static List<string> PreviousCommands => CommanderManager.Instance.PreviousCommands;

        /// <summary>
        /// Apply the command and get the log result
        /// </summary>
        public static void ApplyCommand(string command, out List<string> log)
        {
            CommanderManager.Instance.ApplyCommand(command, out log);
        }

        /// <summary>
        /// Get the name of the commands that is most similar to the entered command
        /// </summary>
        public static string GetSimilarCommand(string command)
        {
            return CommanderManager.Instance.GetSimilarCommand(command);
        }

        /// <summary>
        /// Get names of the commands that are similar to the entered command
        /// </summary>
        public static List<string> GetSimilarCommands(string command)
        {
            return CommanderManager.Instance.GetSimilarCommands(command);
        }

        /// <summary>
        /// Get all classes that have methods with ConsoleCommandAttribute
        /// </summary>
        public static List<string> GetAllConsoleTypes()
        {
            return CommanderManager.Instance.GetAllConsoleTypes();
        }

        /// <summary>
        /// Get commands with or without their descriptions
        /// </summary>
        public static List<string> GetCommands(bool showDescription)
        {
            return CommanderManager.Instance.GetCommands(showDescription);
        }

        /// <summary>
        /// Get commands with or without their descriptions by command flags
        /// </summary>
        public static List<string> GetCommands(bool showDescription, CommandFlags commandFlags)
        {
            return CommanderManager.Instance.GetCommands(showDescription, commandFlags);
        }

        /// <summary>
        /// Print common message in console
        /// </summary>
        public static void Log(object message)
        {
            if (duplicateLog) Debug.Log(message);
            message = message.ToString().SetColor(CommanderSettings.LogColorHtml);
            LogEvent?.Invoke(message.ToString());
        }

        /// <summary>
        /// Print error message in console
        /// </summary>
        public static void LogError(object message)
        {
            if (duplicateLog) Debug.LogError(message);
            message = message.ToString().SetColor(CommanderSettings.LogErrorColorHtml);
            LogEvent?.Invoke(message.ToString());
        }

        /// <summary>
        /// Print warning message in console
        /// </summary>
        public static void LogWarning(object message)
        {
            if (duplicateLog) Debug.LogWarning(message);
            message = message.ToString().SetColor(CommanderSettings.LogWarningColorHtml);
            LogEvent?.Invoke(message.ToString());
        }

        /// <summary>
        /// event triggered when log method is called
        /// </summary>
        public static event Action<string> LogEvent;
    }
}