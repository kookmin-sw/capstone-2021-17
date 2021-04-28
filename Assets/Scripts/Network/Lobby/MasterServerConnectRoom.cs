using UnityEditor;
using UnityEngine;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Games;

public class MasterServerConnectRoom : MonoBehaviour
{
    public static MasterServerConnectRoom Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddMasterServerConnectionListners()
    {
        ClientToMasterConnector.Instance.Connection.AddConnectionListener(OnConnectedToMasterServerHandler, true);
        ClientToMasterConnector.Instance.Connection.AddDisconnectionListener(OnDisconnectedFromMasterServerHandler, false);
    }

    public void OnConnectedToMasterServerHandler()
    {
        MstTimer.WaitForSeconds(0.2f, () =>
        {
            Mst.Client.Matchmaker.FindGames((games) =>
            {
                foreach(var game in games)
                {
                    if (game.OnlinePlayers != game.MaxPlayers) // and game is not started
                        Debug.Log("Game Name : " + game.Name + "\n game players :" + game.OnlinePlayers);
                    MatchmakingBehaviour.Instance.StartMatch(game);
                    
                    break;
                }
            });
        });
    }

    public void OnDisconnectedFromMasterServerHandler()
    {

    }
}
