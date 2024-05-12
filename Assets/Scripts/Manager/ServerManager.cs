
using Controller;
using kcp2k;
using Manager;
using Mirror;
using UnityEngine;

public class ServerManager : Singleton<ServerManager>
{
    #region config
    
    private bool runInBackground = false;
    public NetworkManagerMode mode { get; private set; }
    public Transport transport;
    
    // server
    private int maxConnections = 100;
    public int sendRate = 30;
    public GameObject playerPrefab;
    public bool exceptionsDisconnect = true;
    
    // client
    string networkAddress = "localhost";
    
    #endregion
    
    public ServerManager()
    {
        playerPrefab = PlayerManager.Get().playerPrefab;
        GameObject obj = new GameObject("transport");
        transport = obj.AddComponent<KcpTransport>();
        Transport.active = transport;
    }
    
    public void StartClient()
    {
        // Do checks and short circuits before setting anything up.
        // If / when we retry, we won't have conflict issues.
        if (NetworkClient.active)
        {
            LogManager.LogWarning("Client already started.");
            return;
        }

        mode = NetworkManagerMode.ClientOnly;

        SetupClient();

        // In case this is a headless client...
        ConfigureHeadlessFrameRate();

        RegisterClientMessages();

        NetworkClient.Connect(networkAddress);
    }
    
    public void StartHost()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            LogManager.LogWarning("Server or Client already started.");
            return;
        }

        mode = NetworkManagerMode.Host;

        SetupServer();
        LogManager.Log("Server Setup Finish");
        
        FinishStartHost();
    }
    
    void FinishStartHost()
    {
        NetworkClient.ConnectHost();
        NetworkServer.SpawnObjects();

        SetupClient();
        RegisterClientMessages();
        LogManager.Log("Client Setup Finish");
        HostMode.InvokeOnConnected();
        
        LogManager.Log("Host Start Finish");

        // if (NetworkClient.isConnected && !NetworkClient.ready)
        // {
        //     NetworkClient.Ready();
        //     if (NetworkClient.localPlayer == null)
        //         NetworkClient.AddPlayer();
        // }
    }


    #region Server
    
    // full server setup code, without spawning objects yet
    private void SetupServer()
    {
        // apply settings before initializing anything
        // NetworkServer.disconnectInactiveConnections = disconnectInactiveConnections;
        // NetworkServer.disconnectInactiveTimeout = disconnectInactiveTimeout;
        // NetworkServer.exceptionsDisconnect = exceptionsDisconnect;

        if (runInBackground)
            Application.runInBackground = true;
        
        ConfigureHeadlessFrameRate();

        // start listening to network connections
        NetworkServer.Listen(maxConnections);

        // this must be after Listen(), since that registers the default message handlers
        RegisterServerMessages();
    }
    
    public void ConfigureHeadlessFrameRate()
    {
        if (Mirror.Utils.IsHeadless())
        {
            Application.targetFrameRate = sendRate;
            // Debug.Log($"Server Tick Rate set to {Application.targetFrameRate} Hz.");
        }
    }
    
    void RegisterServerMessages()
    {
        NetworkServer.OnConnectedEvent = OnServerConnectInternal;
        NetworkServer.OnDisconnectedEvent = OnServerDisconnect;
        NetworkServer.RegisterHandler<AddPlayerMessage>(OnServerAddPlayerInternal);

        // Network Server initially registers its own handler for this, so we replace it here.
        NetworkServer.ReplaceHandler<ReadyMessage>(OnServerReadyMessageInternal);
    }
    
    void OnServerReadyMessageInternal(NetworkConnectionToClient conn, ReadyMessage msg)
    {
        //Debug.Log("NetworkManager.OnServerReadyMessageInternal");
        if (conn.identity == null)
        {
            // this is now allowed (was not for a while)
            //Debug.Log("Ready with no player object");
        }
        NetworkServer.SetClientReady(conn);
    }
    
    void OnServerAddPlayerInternal(NetworkConnectionToClient conn, AddPlayerMessage msg)
    {
        LogManager.Log("OnServerAddPlayerInternal");
        if (conn.identity != null)
        {
            LogManager.LogError("There is already a player for this connection.");
            return;
        }

        PlayerController playerController;
        
        playerController = PlayerManager.Get().CreatePlayer(conn.connectionId);
        string name = "player";
        // LogManager.LogError(conn.connectionId);
        playerController.ServerSetupPlayer($"{name} [connId={conn.connectionId}]");
        NetworkServer.AddPlayerForConnection(conn, playerController.GetTransform().gameObject);
    }
    
    void OnServerConnectInternal(NetworkConnectionToClient conn)
    {
        conn.isAuthenticated = true;
    }
    
    public void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        // by default, this function destroys the connection's player.
        // can be overwritten for cases like delayed logouts in MMOs to
        // avoid players escaping from PvP situations by logging out.
        NetworkServer.DestroyPlayerForConnection(conn);
        //Debug.Log("OnServerDisconnect: Client disconnected.");
    }
    
    #endregion

    
    #region Client

    void SetupClient()
    {
        // apply settings before initializing anything
        NetworkClient.exceptionsDisconnect = exceptionsDisconnect;
        // NetworkClient.sendRate = clientSendRate;
    }
    
    void RegisterClientMessages()
    {
        NetworkClient.OnConnectedEvent = OnClientConnectInternal;
        NetworkClient.OnDisconnectedEvent = OnClientDisconnectInternal;
        // Don't require authentication because server may send NotReadyMessage from ServerChangeScene
        NetworkClient.RegisterHandler<NotReadyMessage>(OnClientNotReadyMessageInternal, false);

        if (playerPrefab != null)
            NetworkClient.RegisterPrefab(playerPrefab);
    }
    
    void OnClientConnectInternal()
    {
        NetworkClient.connection.isAuthenticated = true;

        if (!NetworkClient.ready)
            NetworkClient.Ready();
        NetworkClient.AddPlayer();
        
        SceneManager.Get().EnterScene(Const.SceneID.SCENE1);
    }
    
    void OnClientDisconnectInternal()
    {
        if (mode == NetworkManagerMode.ServerOnly || mode == NetworkManagerMode.Offline)
            return;

        if (mode == NetworkManagerMode.Host)
            mode = NetworkManagerMode.ServerOnly;
        else
            mode = NetworkManagerMode.Offline;

        // shutdown client
        NetworkClient.Shutdown();

        // Exit here if we're now in ServerOnly mode (StopClient called in Host mode).
        if (mode == NetworkManagerMode.ServerOnly) return;
        
    }
    
    void OnClientNotReadyMessageInternal(NotReadyMessage msg)
    {
        NetworkClient.ready = false;
    }

    #endregion
    
}