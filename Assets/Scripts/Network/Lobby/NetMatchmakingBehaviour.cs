using UnityEditor;
using UnityEngine;
using MasterServerToolkit.Games;

public class NetMatchmakingBehaviour : MatchmakingBehaviour
{

    protected override void StartLoadingGameScene()
    {
        NetManager.instance.StartClient();
    }
}

