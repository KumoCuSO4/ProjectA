using System.Collections.Generic;
using Controller;
using Mirror;
using UnityEngine;
using Utils;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController localPlayer;
    private Dictionary<int, PlayerController> otherPlayers = new Dictionary<int, PlayerController>();
    private int _playerNum = 0;
    public GameObject playerPrefab { get; private set; }
    
    public PlayerManager()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/player");
        // LogManager.Log("PlayerManager created");
    }
    
    public PlayerController CreatePlayer(int id)
    {
        LogManager.Log("CreateLocalPlayer", id);
        GameObject player = Object.Instantiate(playerPrefab, Vector3.up * 2, Quaternion.identity);
        PlayerController playerController = new PlayerController(player.transform);
        _playerNum++;
        return playerController;
    }
    
    public bool AddLocalPlayer(PlayerController playerController)
    {
        if (localPlayer != null)
        {
            LogManager.TraceE("LocalPlayer Set Duplicate");
            return false;
        }

        localPlayer = playerController;
        return true;
    }

    public PlayerController GetLocalPlayer()
    {
        return localPlayer;
    }

    public bool AddOtherPlayer(PlayerController playerController)
    {
        int id = playerController.GetID();
        if (otherPlayers.ContainsKey(id))
        {
            LogManager.TraceE("OtherPlayer Duplicate");
            return false;
        }

        otherPlayers[id] = playerController;
        return true;
    }

    public PlayerController GetOtherPlayer(int id)
    {
        return otherPlayers[id];
    }
}