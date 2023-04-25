using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace dada.Console
{
    public class ConsoleManager
    {
        public static string[] fileNames = { "Assets" };

        #region Instance
        static ConsoleManager instance;
        public static ConsoleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConsoleManager();
                    instance.Initialize();
                }
                return instance;
            }
        }
        #endregion

        public List<string> commands = new List<string>();
        public List<string> commandsDescriptions = new List<string>();

        List<CashedType> cashedTypes = new List<CashedType>();

        string errorColor = "red";

        /// <summary>
        /// Apply the command and get the log result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        public void ApllyCommand(string command, out List<string> result)
        {
            bool match = false;
            result = new List<string>();
            string commandCheck;

            ParseCommand(command, out string methodName, out string[] parameters);

            #region set parameters
            List<object> parametersList = new List<object>();
            string parametersString = "";
            if (parameters != null)
            {
                foreach (string param in parameters)
                {
                    if (int.TryParse(param, out int resInt))
                    {
                        parametersList.Add(resInt);
                    }
                    else if (float.TryParse(param, out float resFloat))
                    {
                        parametersList.Add(resFloat);
                    }
                    else
                    {
                        parametersList.Add(param);
                    }
                    parametersString += $"{param} ";
                }
            }
            object[] parametersArray = parametersList.Count != 0 ? parametersList.ToArray() : null;
            #endregion

            foreach (var cahsedType in cashedTypes)
            {
                foreach (var method in cahsedType.methods)
                {
                    commandCheck = GetCommandName(method);
                    if (methodName == commandCheck)
                    {
                        bool successInvoke = false;
                        match = true;
                        if (!method.IsStatic)
                        {
                            UnityEngine.Object objType = UnityEngine.Object.FindFirstObjectByType(cahsedType.type);
                            if (objType != null)
                            {
                                successInvoke = InvokeMethod(method, ref result, objType, parametersArray);
                            }
                            else
                            {
                                result.Add($"<color={errorColor}>Object {cahsedType} is don`t exist</color>");
                                return;
                            }
                        }
                        else
                        {
                            successInvoke = InvokeMethod(method, ref result, cahsedType, parametersArray);
                        }
                        if (successInvoke)
                        {
                            string logResult = method.GetCustomAttribute<ConsoleCommandAttribute>().logResult;
                            if (logResult != "") result.Add(logResult);
                        }
                    }
                }
            }
            if (!match)
            {
                result.Clear();
                result.Add($"<color={errorColor}>this command not exist</color>");
            }
        }
        bool InvokeMethod(MethodInfo method, ref List<string> result, object obj, object[] parameters)
        {
            ParameterInfo[] parameterInfo = method.GetParameters();
            if(parameters != null)
            {
                if (parameters.Length != parameterInfo.Length)
                {
                    result = new List<string>() { $"<color={errorColor}>method has not this parameters</color>" };
                    return false;
                }
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!System.Object.ReferenceEquals(parameterInfo[i].ParameterType, parameters[i].GetType()))
                    {
                        if (!(System.Object.ReferenceEquals(parameters[i].GetType(), typeof(int)) &&
                            System.Object.ReferenceEquals(parameterInfo[i].ParameterType, typeof(float))))
                        {
                            result = new List<string>() { $"<color={errorColor}>method has not this parameters</color>" };
                            return false;
                        }
                    }
                }
            }
            else if (parameters == null && parameterInfo.Length != 0)
            {
                result = new List<string>() { $"<color={errorColor}>method needs parameters:</color> \n" };
                foreach(ParameterInfo info in parameterInfo)
                {
                    result[0] += $"{info.Position + 1}: ({info.ParameterType}) {info.Name}";
                    if (info.Position + 1 != parameterInfo.Length) result[0] += '\n';
                }
                return false;
            }
            if (method.ReturnType == typeof(void))
            {
                method.Invoke(obj, parameters);
                return true;
            }
            if (method.ReturnType == typeof(string))
            {
                result.Add(method.Invoke(obj, parameters) as string);
                return true;
            }
            if (method.ReturnType == typeof(List<string>))
            {
                result.AddRange(method.Invoke(obj, parameters) as List<string>);
                return true;
            }
            if (method.ReturnType == typeof(string[]))
            {
                result.AddRange(method.Invoke(obj, parameters) as string[]);
                return true;
            }
            result = new List<string>() { $"<color={errorColor}>method has unsupported return value</color>" };
            return false;
        }
        void ParseCommand(string command, out string methodName, out string[] parameters)
        {
            methodName = null;
            parameters = null;
            int coincidenceCount = 0;
            string currenCommand = null;
            foreach (string _cmd in commands)
            {
                if (command.StartsWith(_cmd))
                {
                    if (_cmd.Length == command.Length)
                    {
                        methodName = _cmd;
                        return;
                    }
                    if (_cmd.Length > coincidenceCount)
                    {
                        coincidenceCount = _cmd.Length;
                        currenCommand = _cmd;
                    }
                }
            }
            if (currenCommand == null) return;
            methodName = currenCommand;
            command = command.Remove(0, methodName.Length);
            if (command[0] != ' ')
            {
                methodName = null;
                return;
            }
            parameters = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Get the name of the command that is most similar to the entered command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string GetSimilarCommand(string command)
        {
            int coincidenceCount = int.MaxValue;
            string resultCommand = null;
            foreach (string cmd in commands)
            {
                if (cmd.StartsWith(command))
                {
                    if (cmd.Length < coincidenceCount)
                    {
                        coincidenceCount = cmd.Length;
                        resultCommand = cmd;
                    }
                }
            }
            return resultCommand;
        }

        /// <summary>
        /// Get all classes that have methods with ConsoleCommandAttribute
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllConsoleTypes()
        {
            List<string> types = new List<string>();
            foreach (CashedType cashedType in cashedTypes)
            {
                string typeInfo = $"class {cashedType.type.Name} has {cashedType.methods.Count()} console methods: \n";
                foreach (MethodInfo method in cashedType.methods)
                {
                    typeInfo += $"{method.Name} \n";
                }
                types.Add(typeInfo);
            }
            return types;
        }

        /// <summary>
        /// Get commands by parameters
        /// </summary>
        /// <param name="showDescription"></param>
        /// <param name="commandFlags"></param>
        /// <returns></returns>
        public List<string> GetCommands(bool showDescription, CommandFlags commandFlags = CommandFlags.all)
        {
            List<string> _cmds = new List<string>();
            foreach (CashedType cashedType in cashedTypes)
            {
                foreach (MethodInfo method in cashedType.methods)
                {
                    if ((method.GetCustomAttribute<ConsoleCommandAttribute>().commandFlag & commandFlags) == 0) continue;
                    string str = method.GetCustomAttribute<ConsoleCommandAttribute>().commandName;
                    if (str == "") str = method.Name;
                    if (showDescription)
                    {
                        string _description = method.GetCustomAttribute<ConsoleCommandAttribute>().description;
                        if (_description != "") str += $" - {_description}";
                    }
                    _cmds.Add(str);
                }
            }
            return _cmds;
        }
        string GetCommandName(MethodInfo method)
        {
            return method.GetCustomAttribute<ConsoleCommandAttribute>().commandName != "" ?
                        method.GetCustomAttribute<ConsoleCommandAttribute>().commandName :
                        method.Name;
        }

        void Initialize()
        {
            string appPath = Application.dataPath.Remove(Application.dataPath.Length - 7).Replace('/', '\\');

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().
                Where(a => a.Location.StartsWith(appPath)).
                Where(a => !a.GetName().Name.StartsWith("Unity.")).
                Where(a => !a.GetName().Name.StartsWith("System.")).
                Where(a => !a.GetName().Name.StartsWith("UnityEditor.")).
                Where(a => !a.GetName().Name.StartsWith("UnityEngine.")).ToArray();

            foreach (Assembly assembly in assemblies)
            {
                var types = assembly.GetTypes();

                BindingFlags flags =
                      BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.DeclaredOnly;
                foreach (var type in types)
                {
                    var methods = type.GetMethods(flags).
                        Where(m => m.GetCustomAttributes<ConsoleCommandAttribute>().Count() > 0);
                    if (methods.Count() == 0) continue;
                    cashedTypes.Add(new CashedType(type, methods));
                    foreach (var method in methods)
                    {
                        commands.Add(GetCommandName(method));
                        commandsDescriptions.Add(method.GetCustomAttribute<ConsoleCommandAttribute>().description);
                    }
                }
            }
        }

        private class CashedType
        {
            public Type type;
            public IEnumerable<MethodInfo> methods;

            public CashedType(Type type, IEnumerable<MethodInfo> methods)
            {
                this.type = type;
                this.methods = methods;
            }
        }
    }
}
