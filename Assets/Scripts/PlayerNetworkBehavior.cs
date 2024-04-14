using Controller;
using Mirror;
using UnityEngine;

public class PlayerNetworkBehavior : NetworkBehaviour
{
    private PlayerController _playerController;
    private NetworkIdentity _identity;
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
            int id = Random.Range(1, 100); // TODO
            LogManager.LogError(id);
            _playerController = new PlayerController(transform);
            _playerController.SetID(id);
            _playerController.OnPlayerSpawn(isLocalPlayer);
        }
        else
        {
            _playerController.OnPlayerSpawn(isLocalPlayer);
        }
    }
}