using System;

namespace Dada.Commander
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class ConsoleCommandAttribute : Attribute
    {
        public string commandName = "";
        public string description = "";
        public string logResult = "";

        public CommandFlags commandFlag = CommandFlags.noFlags;
    }
}