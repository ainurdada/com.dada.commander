using System;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommandAttribute : Attribute
{
    public string commandName = "";
    public string description = "";
    public string logResult = "";

    public CommandFlags commandFlag = CommandFlags.noFlags;
}