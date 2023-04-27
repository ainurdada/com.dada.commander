using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Dada.Commander.Core
{
    internal class CommanderManager
    {
        public static string[] fileNames = { "Assets" };

        #region Instance
        static CommanderManager instance;
        public static CommanderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommanderManager();
                    instance.Initialize();
                }
                return instance;
            }
        }
        #endregion

        public List<string> commands = new List<string>();
        public List<string> commandsDescriptions = new List<string>();

        List<CashedType> cashedTypes = new List<CashedType>();

        public string textColor = "white";
        public string errorColor = "red";

        /// <summary>
        /// Apply the command and get the log result
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result"></param>
        public void ApplyCommand(string command, out List<string> result)
        {
            bool match = false;
            result = new List<string>();

            ParseCommand(command, out string commandName, out string[] parameters);

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

            Func<CashedType, MemberInfo> _GetMember = (t) =>
            {
                var member = t.GetMembers().FirstOrDefault(
                    m => GetCommandName(m) == commandName);

                return member;
            };

            foreach (var type in cashedTypes)
            {
                var member = _GetMember(type);
                if (member != null)
                {
                    bool successInvoke = false;
                    match = true;
                    if (member is MethodInfo method)
                    {
                        if (!method.IsStatic)
                        {
                            UnityEngine.Object objType = UnityEngine.Object.FindFirstObjectByType(type.type);
                            if (objType != null)
                            {
                                successInvoke = InvokeMethod(method, ref result, objType, parametersArray);
                            }
                            else
                            {
                                result.Add($"<color={errorColor}>Object {type} is don`t exist</color>");
                                return;
                            }
                        }
                        else
                        {
                            successInvoke = InvokeMethod(method, ref result, type, parametersArray);
                        }
                    }
                    else if (member is PropertyInfo property)
                    {
                        MethodInfo p_method = null;
                        MethodInfo p_set_method = property.GetSetMethod();
                        MethodInfo p_get_method = property.GetGetMethod();
                        if (p_set_method != null && parametersArray != null) p_method = p_set_method;
                        else if (p_get_method != null) p_method = p_get_method;

                        if (p_method != null)
                        {
                            if (!p_method.IsStatic)
                            {
                                UnityEngine.Object objType = UnityEngine.Object.FindFirstObjectByType(type.type);
                                if (objType != null)
                                {
                                    successInvoke = InvokeMethod(p_method, ref result, objType, parametersArray);
                                }
                                else
                                {
                                    result.Add($"<color={errorColor}>Object {type} is don`t exist</color>");
                                    return;
                                }
                            }
                            else
                            {
                                successInvoke = InvokeMethod(p_method, ref result, type, parametersArray);
                            }
                        }
                    }
                    else if (member is FieldInfo field)
                    {
                        if (!field.IsStatic)
                        {
                            UnityEngine.Object objType = UnityEngine.Object.FindFirstObjectByType(type.type);
                            if (objType != null)
                            {
                                successInvoke = ApplyFieldValue(field, ref result, objType, parametersArray);
                            }
                            else
                            {
                                result.Add($"<color={errorColor}>Object {type} is don`t exist</color>");
                                return;
                            }
                        }
                        else
                        {
                            successInvoke = ApplyFieldValue(field, ref result, type, parametersArray);
                        }
                    }
                    if (successInvoke)
                    {
                        string logResult = member.GetCustomAttribute<ConsoleCommandAttribute>().logResult;
                        if (logResult != "") result.Add(logResult);
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
            if (parameters != null)
            {
                if (parameters.Length != parameterInfo.Length)
                {
                    result = new List<string>() { $"<color={errorColor}>incorrect number of parameters</color>" };
                    return false;
                }
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!System.Object.ReferenceEquals(parameterInfo[i].ParameterType, parameters[i].GetType()))
                    {
                        if (!(System.Object.ReferenceEquals(parameters[i].GetType(), typeof(int)) &&
                            System.Object.ReferenceEquals(parameterInfo[i].ParameterType, typeof(float))))
                        {
                            result = new List<string>() { $"<color={errorColor}>incorrect parameter type</color>" };
                            return false;
                        }
                    }
                }
            }
            else if (parameters == null && parameterInfo.Length != 0)
            {
                result = new List<string>() { $"<color={errorColor}>method needs parameters:</color> \n" };
                foreach (ParameterInfo info in parameterInfo)
                {
                    result[0] += $"<color={textColor}>{info.Position + 1}: ({info.ParameterType}) {info.Name}</color>";
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
                result.Add($"<color={textColor}>{method.Invoke(obj, parameters) as string}</color>");
                return true;
            }
            if (method.ReturnType == typeof(List<string>))
            {
                foreach (string str in method.Invoke(obj, parameters) as List<string>)
                {
                    result.Add($"<color={textColor}>{str}</color>");
                }
                return true;
            }
            if (method.ReturnType == typeof(string[]))
            {
                foreach (string str in method.Invoke(obj, parameters) as string[])
                {
                    result.Add($"<color={textColor}>{str}</color>");
                }
                return true;
            }
            result = new List<string>() { $"<color={errorColor}>method has unsupported return value</color>" };
            return false;
        }
        bool ApplyFieldValue(FieldInfo field, ref List<string> result, object obj, object[] parameters)
        {
            object parameter = null;
            if (parameters != null )
            {
                if (parameters.Count() != 1)
                {
                    result = new List<string>() { $"<color={errorColor}>too much values</color>" };
                    return false;
                }
                else
                {
                    parameter = parameters[0];
                }
            }
            if (parameter == null)
            {
                object fielValue = field.GetValue(obj);
                string str_fieldValue = fielValue != null ? fielValue.ToString() : "null";
                result.Add($"{field.Name} = {str_fieldValue}");
                return true;
            }
            else
            {
                if (!System.Object.ReferenceEquals(field.FieldType, parameter.GetType()))
                {
                    if (!(System.Object.ReferenceEquals(parameter.GetType(), typeof(int)) &&
                        System.Object.ReferenceEquals(field.FieldType, typeof(float))))
                    {
                        result = new List<string>() { $"<color={errorColor}>incorrect type for this field</color>" };
                        return false;
                    }
                }
                field.SetValue(obj, parameter);
                return true;
            }
        }
        void ParseCommand(string command, out string commandName, out string[] parameters)
        {
            commandName = null;
            parameters = null;
            int coincidenceCount = 0;
            string currenCommand = null;
            foreach (string _cmd in commands)
            {
                if (command.StartsWith(_cmd))
                {
                    if (_cmd.Length == command.Length)
                    {
                        commandName = _cmd;
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
            commandName = currenCommand;
            command = command.Remove(0, commandName.Length);
            if (command[0] != ' ')
            {
                commandName = null;
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
                string typeInfo = $"<color={textColor}>class {cashedType.type.Name} has {cashedType.methods.Count()} console methods:</color> \n";
                foreach (MethodInfo method in cashedType.methods)
                {
                    typeInfo += $"<color={textColor}>{method.Name}</color> \n";
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
                var members = cashedType.GetMembers();
                foreach (MemberInfo member in members)
                {
                    if ((member.GetCustomAttribute<ConsoleCommandAttribute>().commandFlag & commandFlags) == 0) continue;
                    string str = member.GetCustomAttribute<ConsoleCommandAttribute>().commandName;
                    if (str == "") str = member.Name;
                    str = $"<color={textColor}>{str}</color>";
                    if (showDescription)
                    {
                        string _description = member.GetCustomAttribute<ConsoleCommandAttribute>().description;
                        if (_description != "") str += $"<color={textColor}> - {_description}</color>";
                    }
                    _cmds.Add(str);
                }
            }
            return _cmds;
        }
        string GetCommandName(MemberInfo member)
        {
            return member.GetCustomAttribute<ConsoleCommandAttribute>().commandName != "" ?
                        member.GetCustomAttribute<ConsoleCommandAttribute>().commandName :
                        member.Name;
        }

        void Initialize()
        {
            string appPath = Application.dataPath.Remove(Application.dataPath.Length - 7).Replace('/', '\\');

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().
                Where(a => !a.IsDynamic).
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

                    var properties = type.GetProperties(flags).
                        Where((m) => m.GetCustomAttributes<ConsoleCommandAttribute>().Count() > 0);

                    var fields = type.GetFields(flags).
                        Where((m) => m.GetCustomAttributes<ConsoleCommandAttribute>().Count() > 0);

                    if (methods.Count() != 0 || properties.Count() != 0 || fields.Count() != 0)
                    {
                        cashedTypes.Add(new CashedType(type, methods, properties, fields));
                        foreach (var method in methods)
                        {
                            commands.Add(GetCommandName(method));
                            commandsDescriptions.Add(method.GetCustomAttribute<ConsoleCommandAttribute>().description);
                        }
                        foreach (var property in properties)
                        {
                            commands.Add(GetCommandName(property));
                            commandsDescriptions.Add(property.GetCustomAttribute<ConsoleCommandAttribute>().description);
                        }
                        foreach (var field in fields)
                        {
                            commands.Add(GetCommandName(field));
                            commandsDescriptions.Add(field.GetCustomAttribute<ConsoleCommandAttribute>().description);
                        }
                    }
                }
            }
        }

        private class CashedType
        {
            public Type type;
            public IEnumerable<MethodInfo> methods;
            public IEnumerable<PropertyInfo> properties;
            public IEnumerable<FieldInfo> fields;

            public CashedType(Type type, IEnumerable<MethodInfo> methods, IEnumerable<PropertyInfo> properties, IEnumerable<FieldInfo> fields)
            {
                this.type = type;
                this.methods = methods;
                this.properties = properties;
                this.fields = fields;
            }

            public MemberInfo[] GetMembers()
            {
                var members = methods.Select((m) => m as MemberInfo).ToList();
                members.AddRange(properties.Select((p) => p as MemberInfo).ToList());
                members.AddRange(fields.Select((f) => f as MemberInfo).ToList());

                return members.ToArray();
            }
        }
    }
}
