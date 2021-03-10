using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DebugInGameNetManager : NetworkManager
{
    private GamePlay_MultiGameManager gameManager;

    public override void OnStartClient()
    {
        gameManager = GamePlay_MultiGameManager.instance;
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        RegisterStartPosition(gameManager.GetSpawnPoint());
        base.OnServerAddPlayer(conn);
        
        
    }

    

    


}
