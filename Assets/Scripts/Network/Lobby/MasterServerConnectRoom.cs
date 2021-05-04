using UnityEditor;
using UnityEngine;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using MasterServerToolkit.Games;

public class MasterServerConnectRoom : MonoBehaviour
{
    public static MasterServerConnectRoom Instance;

    public string RoomName;

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

    
}
