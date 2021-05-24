using UnityEngine;

/// <summary>
/// Attach this to your network manager. Handles commands.
/// NOTE: This only works for windows servers!
/// </summary>
public class ServerConsole : MonoBehaviour
{
#if UNITY_STANDALONE_WIN && UNITY_SERVER

	Cooper.GarryTV.Windows.ConsoleWindow console = new Cooper.GarryTV.Windows.ConsoleWindow();
	Cooper.GarryTV.Windows.ConsoleInput input = new Cooper.GarryTV.Windows.ConsoleInput();

	CommandModule commandModule = new CommandModule();

    void Update() => input.Update();
	void OnDestroy() => console.Shutdown();

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		console.Initialize();
		console.SetTitle("Mirror Server Build");

		input.OnInputText += OnInputText;
		Application.logMessageReceived += HandleLog;

		commandModule.GetCommands();
	}

	void OnInputText(string obj)
	{
		var enteredArgs = obj.Split(' ');
		var prefix = enteredArgs[0];

		// for a command with no args
		if (enteredArgs.Length == 1)
			commandModule.ExecuteCommand(prefix, enteredArgs);
		else
			commandModule.ExecuteCommand(prefix, enteredArgs.Skip(1).ToArray());
	}

	void HandleLog(string message, string stackTrace, LogType type)
	{
		if (type == LogType.Warning)
			System.Console.ForegroundColor = ConsoleColor.Yellow;
		else if (type == LogType.Error)
			System.Console.ForegroundColor = ConsoleColor.Red;
		else
			System.Console.ForegroundColor = ConsoleColor.White;
	}
#endif
}