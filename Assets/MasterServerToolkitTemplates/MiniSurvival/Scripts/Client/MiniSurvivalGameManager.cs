using MasterServerToolkit.Bridges.MirrorNetworking.Character;
using MasterServerToolkit.Games;
using MasterServerToolkit.MasterServer;
using MasterServerToolkit.Networking;
using Mirror;
using UnityEngine;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class MiniSurvivalGameManager : MonoBehaviour
    {
        /// <summary>
        /// Client side player character
        /// </summary>
        protected PlayerCharacterVitals playerCharacterVitals;

        protected void Awake()
        {
            Mst.Events.AddEventListener(MstEventKeys.playerStartedGame, OnPlayerStartedGameEventHandler);
            Mst.Events.AddEventListener(MstEventKeys.playerFinishedGame, OnPlayerFinishedGameEventHandler);
        }

        protected void OnDestroy()
        {
            Mst.Events.RemoveEventListener(MstEventKeys.playerStartedGame, OnPlayerStartedGameEventHandler);
            Mst.Events.RemoveEventListener(MstEventKeys.playerFinishedGame, OnPlayerFinishedGameEventHandler);
        }

        protected virtual void OnPlayerStartedGameEventHandler(EventMessage message)
        {
            playerCharacterVitals = message.GetData<PlayerCharacter>().GetComponent<PlayerCharacterVitals>();
            playerCharacterVitals.OnDieEvent += OnDieEventHandler;
        }

        protected virtual void OnPlayerFinishedGameEventHandler(EventMessage message)
        {
            if (playerCharacterVitals)
            {
                playerCharacterVitals.OnDieEvent -= OnDieEventHandler;
            }
        }

        protected virtual void OnDieEventHandler()
        {
            MstTimer.WaitForSeconds(3f, () =>
            {
                Mst.Events.Invoke(MstEventKeys.showYesNoDialogBox,
                    new YesNoDialogBoxEventMessage("You are dead! Would you like to play this game again?",
                    () => {
                        Bridges.MirrorNetworking.RoomClient.Instance.CreatePlayer();
                    },
                    () => {
                        EndGame();
                    }));
            });
        }

        public void EndGame()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}