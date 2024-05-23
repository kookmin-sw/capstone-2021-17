using UnityEditor;
using UnityEngine;
using Mirror;

public class GameMgrNetBehaviour : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnClearCountChanged))] 
    public int missionClearCount;

    private GameMgr gameMgr;

    private void Start()
    {
        gameMgr = GameMgr.instance;
    }

    public void AddMissionClearCount()
    {
        CmdAddMissionClearCount();
    }

    [Command(requiresAuthority =false)]
    public void CmdAddMissionClearCount()
    {
        missionClearCount++;

        if (missionClearCount == gameMgr.missionSpawnCount)
        {
            gameMgr.ActiveExitDoor();
        }
    }

    [ClientRpc]
    public void RpcChangeLeverLayer(int i)
    {
        gameMgr.ChangeLeverLayer(i);
    }

    void OnClearCountChanged(int oldValue, int newValue) // SyncVar가 바뀔 때마다 호출됨
    {
        missionClearCount = newValue;
        gameMgr.missionClearCount = missionClearCount;
    } 
}