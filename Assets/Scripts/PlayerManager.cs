using System.Collections.Generic;
using Controller;
using UnityEngine;
using Utils;

public class PlayerManager : Singleton<PlayerManager>
{
    private PlayerController localPlayer;
    private List<PlayerController> otherPlayers = new List<PlayerController>();
    private int _playerNum = 0;
    public PlayerManager()
    {
        LogManager.Log("PlayerManager created");
    }
    
    public void SetLocalPlayer(PlayerController player)
    {
        if (localPlayer)
        {
            LogManager.TraceE("LocalPlayer Set Duplicate");
            return;
        }
        LogManager.Log("SetLocalPlayer");
        localPlayer = player;
        player.SetIndex(_playerNum);
        _playerNum++;
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