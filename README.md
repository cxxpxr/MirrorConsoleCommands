# MirrorConsoleCommands
A simple console command addon for Mirror.

# What
This is an addon for mirror that allows commands to be entered into the console.

# Limitations
This currently only works for Windows server builds!

# How to use
1. Install the latest release into your project.
2. Add ServerConsole.cs to your NetworkManager.
3. Add the following Attribute: [Cooper.ConsoleCommand("CommandNameHere")] to your method
4. Take a look at the example commands in DefaultCommands.cs
5. Profit!

# Special Thanks
Thank you to Garry Newman, who provided some of the boilerplate code for allowing input into the console.
See his blog: https://garry.tv/unity-batchmode-console
