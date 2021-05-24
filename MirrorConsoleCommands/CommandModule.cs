using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CommandModule
{
    private Dictionary<string, MethodInfo> _commands = new Dictionary<string, MethodInfo>();

    public CommandModule() { }

    public void GetCommands()
    {
         var methods = AppDomain.CurrentDomain.GetAssemblies() 
        .SelectMany(x => x.GetTypes()) 
        .Where(x => x.IsClass) 
        .SelectMany(x => x.GetMethods()) 
        .Where(x => x.GetCustomAttributes(typeof(Cooper.ConsoleCommand), false).FirstOrDefault() != null);

        foreach (var item in methods)
        {
            var parameters = item.GetParameters();

            if(parameters.Length > 1 || parameters[0].ParameterType != typeof(string[]))
            {
                Debug.LogWarning($"CONSOLE COMMANDS | Skipping command with prefix '" + item.Name + "' because it has incorrect paramaters!\n" +
                    " Please make sure your command's method's parameters are only a string array!");
                continue;
            }

            if(item.ReturnType != typeof(void))
            {
                Debug.LogWarning("CONSOLE COMMANDS | Skipping command with prefix '" + item.Name + "' because it has incorrect return type!\n" +
    " Please make sure your command's return type is void!");
                continue;
            }

            var attribute = (Cooper.ConsoleCommand)item.GetCustomAttribute(typeof(Cooper.ConsoleCommand));

            if(attribute != null)
            {
                _commands.Add(attribute.GetPrefix(), item);
            }
        }

        Debug.Log("CONSOLE COMMANDS | Loaded " + _commands.Count + " commands!");
    }

    public void ExecuteCommand(string parsedPrefix, string[] args)
    {
        if(_commands.TryGetValue(parsedPrefix, out MethodInfo method))
        {
            object[] parameters = new object[] { args };
            var obj = Activator.CreateInstance(method.DeclaringType);

            try
            {
                method.Invoke(obj, parameters);
                Debug.Log($"CONSOLE COMMANDS | Command executed successfully!");
            }
            catch
            {
                Debug.LogError("CONSOLE COMMANDS | Command execution failed!");
            }
        }
    }
}

namespace Cooper
{
    public class ConsoleCommand : Attribute
    {
        private string prefix;
        public string GetPrefix() => prefix;

        public ConsoleCommand(string prefix) => this.prefix = prefix;
    }
}
