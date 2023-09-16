<p align="center">
<img src="https://i.ibb.co/QC7pfJZ/Commander-Label800-new.png">
</p>

<p align="center">
<img src="https://img.shields.io/badge/Unity-white?style=flat&logo=unity&logoColor=000000">
<img src="https://img.shields.io/badge/Git repo-2.0.0-success">
<img src="https://img.shields.io/badge/Release-2.0.0-success">
<img src="https://img.shields.io/badge/License-MIT-success">
</p>

## About
Commander is a developer console. Fast, simple and has many features.  

## Installation
|Version:| Instruction|
|----------------|---------------------------|
|Release version:| follow [releases](https://github.com/ainurdada/com.dada.commander/releases)|
Experimental version:| 1. Open `Package Manager` in Unity<br/> 2. Select `Add package from Git URL`<br/> 3. Paste Git URL: `https://github.com/ainurdada/com.dada.commander.git`|

## Get start
For using Commanders API you should use namespace ```Dada.Commander```
1. Add `ConsoleCommand` attribute for your method.  
    Example:
    ```c#
        [ConsoleCommand]    // <-- just add this attribute
        static void SetMaxFps(int fps)
        {
            Application.targetFrameRate = fps;
        }
    ```
2. **Thats all!** Now you can write method's name in the console ui. But there are still a lot of additional settings that will help when working with the commander. About them further.

## Commander UI
Console UI is located on path `Commander\UI\Prefab`
![](https://i.ibb.co/4KGj2Lt/Console-UIPreview.png)  
Use namespace `Dada.Commander.UI` to manage console UI

### `CommanderUI` class:
|Methods|Descripton|
|-------|----------|
|`ChangeOpenCloseState`|Open or close console window|
|`FillAuto`|Autofill inputfield by similar command| 
|`SelectNextHelperItem`|Select next command in PopUp command window|
|`SelectPreviousHelperItem`|Select previous command in PopUp command window|
|`SelectNextLastCommand`|Select an older command|
|`SelectPreviousLastCommand`|Select a newer command|

|Property|Type|Description|
|--------|----|-----------|
|`WindowIsShowed`|`bool`|True if the console is shown and false if not|
|`PopUpHelperIsShowed`|`bool`|True if the PopUp command window is shown and false if not

|Event|Description|
|-----|-----------|
|`OnShowed`|Called when the console is showed|
|`OnHided`|Called when the console is hided|


## Core API
### `ConsoleCommand` attribute:
|Field|Default value|Description|
|-----|-----------|-------------|
|`commandName`|method name|Name for Commander<br/> If you dont set `commandName` it takes name of method|
|`description`||Description for Commander <br/> This description can be shown in Commander's log|
|`logResult`||Log result that will be written to the Commander's log after successful method apllying|
|`commandFlag`|`CommandFlags.noFlags`|Flag that describe type of method|

### `Commander` class:   
|Method|Parameters|Descripton|
|------|----------|----------|
|`ApplyCommand`|`string` command,<br/> `out List<string>` log|Invoke method or field by his command name and get a log result. <br/> `command` - command name and parameters of class member <br/> `log` - log result of applying command|
|`GetSimilarCommand`|`string` command|Get the name of the commands that is most similar to the entered `command`|
|`GetSimilarCommands`|`string` command|Get names of the commands that are similar to the entered command|
|`GetAllConsoleTypes`||Get all class names that have methods with `ConsoleCommand` attribute|
|`GetCommands`|`bool` showDescription|Get all commands with or without their descriptions|
|`GetCommands`|`bool` showDescription <br/> `CommandFlags` commandFlags|Get commands with or without their descriptions that has `commandFlags`|
|`Log`|`string` message|Send message to `LogEvent` event|
|`LogWarning`|`string` message|Send message to `LogEvent` event|
|`LogError`|`string` message|Send message to `LogEvent` event|

|Event|Callback type|Description|
|-----|----------|-----------|
|`LogEvent`|`string`| Called when Log method invokes and give a log message in callback|

|Property|Type|Description|
|--------|----|-----------|
|`PreviousCommands`|`List<string>`|List of previous commands, that have been sent to Commander|

## Custom flags
You can create your own custom flag set in `com.dada.commander\Runtime\CommandFlags.cs` file, but you always should have `all` flag that contains all bytes of another flags. 
