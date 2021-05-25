/// <summary>
/// Example commands.
/// If a command fails, you should throw an exception.
/// To make a new command, just write a method like normal and attach the attribute.
/// </summary>
public class DefaultCommands
{
    [Cooper.ConsoleCommand("set-port")]
    public void SetPortCommand(ushort serverPort)
    {
        var transport = Mirror.NetworkManager.singleton.GetComponent<Mirror.Transport>();

        if (Mirror.NetworkManager.singleton.mode != Mirror.NetworkManagerMode.Offline)
            throw new System.Exception("Cannot set port because server is running!");

        if (transport == null) throw new System.Exception("Couldn't find a transport!");

        if (transport is kcp2k.KcpTransport kcp)
        {
            kcp.Port = serverPort;
            return;
        }

        if (transport is Mirror.SimpleWeb.SimpleWebTransport swt)
        {
            swt.port = serverPort;
            return;
        }

        if (transport is Mirror.TelepathyTransport tele)
        {
            tele.port = serverPort;
            return;
        }

        throw new System.Exception("This command is not compatible with your current transport!");
    }

    [Cooper.ConsoleCommand("stop")]
    public void ShutdownServer()
    {
        var man = Mirror.NetworkManager.singleton;

        if (man.mode == Mirror.NetworkManagerMode.ServerOnly)
        {
            man.StopServer();
        }
        else
            throw new System.Exception("Cannot stop server because it has not been started!");
    }

    [Cooper.ConsoleCommand("start")]
    public void StartServer()
    {
        var man = Mirror.NetworkManager.singleton;

        if (man.mode == Mirror.NetworkManagerMode.Offline)
        {
            man.StartServer();
        }
        else
            throw new System.Exception("Cannot start server because it is already started!");
    }
}
