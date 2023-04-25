<p align="center">
<img src="https://i.ibb.co/FnmBfL9/Commander-Label800.png">
</p>

<p align="center">
<img src="https://img.shields.io/badge/Version-0.5.0-blue">
<img src="https://img.shields.io/badge/License-MIT-success">
</p>

## About
Commander is a tool for your Unity game that makes it easy to create cheat commands

## Installation
1. Open `Package Manager` in Unity
2. Select `Add package from Git URL`
3. Paste Git URL: `https://github.com/ainurdada/com.dada.commander.git`

## Get start
1. Add `ConsoleCommand` attribute for your method.  
    Example:
    ```c#
        [ConsoleCommand]
        static void SetMaxFps(int fps)
        {
            Application.targetFrameRate = fps;
        }
    ```
2. Now you can manage method with attribute `ConsoleCommand` from any place in your code using `Commander` class.  
    Example:
    ```c#
        //this method invoke method SetMaxFps and give log result
        Commander.ApplyCommand("SetMaxFps", out List<string> log);
    ```

## Methods
`Commander` class has next methods:  
|Method|Parameters|Descripton|
|------|----------|----------|
|`ApplyCommand`|string `command`,<br/> out List\<string> `log`|Invoke method by his command name and get a log result. <br/> `command` - command name of method <br/> `log` - log result of applying command|
|`GetSimilarCommand`|string `command`|Get the name of the commands that is most similar to the entered `command`|
|`GetAllConsoleTypes`||Get all class names that have methods with `ConsoleCommand` attribute|
|`GetCommands`|bool `showDescription`|Get all commands with or without their descriptions|
|`GetCommands`|bool `showDescription` <br/> CommandFlags `commandFlags`|Get commands with or without their descriptions that has `commandFlags`|