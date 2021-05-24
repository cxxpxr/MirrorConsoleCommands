/// <summary>
/// Example commands.
/// If a command fails, you should throw an exception.
/// <para>All commands MUST take in a single parameter, that is a string array of args.</para>
/// <para>You can then parse them as whatever you want in your command.</para>
/// <para>I did it this way because it reduces the code complexity.</para>
/// </summary>
public class DefaultCommands
{
    [Cooper.ConsoleCommand("set-port")]
    public void SetPortCommand(string[] args)
    {
        var transport = Mirror.NetworkManager.singleton.GetComponent<Mirror.Transport>();

        if (Mirror.NetworkManager.singleton.mode != Mirror.NetworkManagerMode.Offline)
            throw new System.Exception("Cannot set port because server is running!");

        if (transport == null) throw new System.Exception("Couldn't find a transport!");

        var portToParse = args[0];

        if (ushort.TryParse(portToParse, out ushort port))
        {
            if (transport is kcp2k.KcpTransport kcp)
            {
                kcp.Port = port;
                return;
            }

            if (transport is Mirror.SimpleWeb.SimpleWebTransport swt)
            {
                swt.port = port;
                return;
            }

            if (transport is Mirror.TelepathyTransport tele)
            {
                tele.port = port;
                return;
            }

            throw new System.Exception("This command is not compatible with your current transport!");
        }
        else
            throw new System.Exception("Couldn't parse port!");
    }

    [Cooper.ConsoleCommand("stop")]
    public void ShutdownServer(string[] args)
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
    public void StartServer(string[] args)
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
