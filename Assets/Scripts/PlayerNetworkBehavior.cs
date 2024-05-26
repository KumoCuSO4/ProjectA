using Controller;
using Mirror;
using Player;
using UnityEngine;

public class PlayerNetworkBehavior : NetworkBehaviour
{
    private PlayerController _playerController;
    private NetworkIdentity _identity;
    
    [SyncVar(hook = nameof(OnPlayerNameChanged))]
    private string _playerName;

    private void OnPlayerNameChanged(string oldStr, string newStr)
    {
        // LogManager.LogError(oldStr, newStr);
        _playerController.OnPlayerNameChanged(oldStr, newStr);
    }

    [Command]
    public void CmdChangeName(string playerName)
    {
        _playerName = playerName;
    }

    [Server]
    public void ServerSetupPlayer(string playerName)
    {
        _playerName = playerName;
    }

    public void SetPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (_playerController == null)
        {
            _identity = GetComponent<NetworkIdentity>();
            _playerController = new PlayerController(gameObject);
        }

        _playerController.OnPlayerSpawn(isLocalPlayer);
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (_playerController == null)
        {
            _identity = GetComponent<NetworkIdentity>();
            _playerController = new PlayerController(gameObject);
        }
        base.OnDeserialize(reader, initialState);
    }
}