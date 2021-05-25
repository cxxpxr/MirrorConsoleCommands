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

            if(item.ReturnType != typeof(void))
            {
                Debug.LogWarning("CONSOLE COMMANDS | Skipping command with prefix '" + item.Name + "' because it has incorrect return type!\n" +
    " Please make sure your command's return type is void!");
                continue;
            }

            var attribute = (Cooper.ConsoleCommand)item.GetCustomAttribute(typeof(Cooper.ConsoleCommand));

            if (attribute.GetPrefix().Contains(" "))
            {
                Debug.LogWarning("CONSOLE COMMANDS | Skipping command with prefix '" + item.Name + "' because its prefix contains spaces!\n");
                continue;
            }

            if (attribute != null)
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
            try
            {
                var requiredMethodParams = method.GetParameters();
                object[] parameters = new object[requiredMethodParams.Length];

                for (int i = 0; i < requiredMethodParams.Length; i++)
                {
                    var para = requiredMethodParams[i];
                    var type = para.ParameterType;

                    parameters[i] = Convert.ChangeType(args[i], type);
                }

                var obj = Activator.CreateInstance(method.DeclaringType);

                method.Invoke(obj, parameters);
                Debug.Log($"CONSOLE COMMANDS | Command executed successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError("CONSOLE COMMANDS | Command execution failed! Stacktrace: " + e);
            }
        }
        else
        {
            Debug.LogError("CONSOLE COMMANDS | Unknown command!");
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
