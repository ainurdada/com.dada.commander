using System.Collections.Generic;
using Dada.Commander.Core;

namespace Dada.Commander
{
    public static class Commander
    {
        /// <summary>
        /// Apply the command and get the log result
        /// </summary>
        public static void ApplyCommand(string command, out List<string> log)
        {
            CommanderManager.Instance.ApplyCommand(command, out log);
        }

        /// <summary>
        /// Get the name of the command that is most similar to the entered command
        /// </summary>
        public static string GetSimilarCommand(string command)
        {
            return CommanderManager.Instance.GetSimilarCommand(command);
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
    }
}