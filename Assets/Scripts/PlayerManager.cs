using System.Collections.Generic;
using Controller;
using Mirror;
using UnityEngine;
using Utils;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController localPlayer;
    private List<PlayerController> otherPlayers = new List<PlayerController>();
    private int _playerNum = 0;
    public GameObject playerPrefab { get; private set; }
    
    public PlayerManager()
    {
        playerPrefab = (GameObject)Resources.Load("Prefabs/player");
        LogManager.Log("PlayerManager created");
    }
    
    public PlayerController CreateLocalPlayer()
    {
        if (localPlayer != null)
        {
            LogManager.TraceE("LocalPlayer Set Duplicate");
            return localPlayer;
        }
        
        LogManager.Log("CreateLocalPlayer");
        GameObject player = Object.Instantiate(playerPrefab, Vector3.up * 2, Quaternion.identity);
        PlayerController playerController = new PlayerController(player.transform, true);
        localPlayer = playerController;
        playerController.SetIndex(_playerNum);
        _playerNum++;
        return playerController;
    }

    public PlayerController GetLocalPlayer()
    {
        return localPlayer;
    }

    public void AddOtherPlayer(PlayerController player)
    {
        LogManager.Log("AddOtherPlayer");
        otherPlayers.Add(player);
        player.SetIndex(_playerNum);
        _playerNum++;
    }

    public List<PlayerController> GetOtherPlayers()
    {
        return otherPlayers;
    }
}