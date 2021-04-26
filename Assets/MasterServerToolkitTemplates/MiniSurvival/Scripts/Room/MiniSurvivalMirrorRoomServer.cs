using MasterServerToolkit.MasterServer;

namespace MasterServerToolkit.Template.MiniSurvival
{
    public class MiniSurvivalMirrorRoomServer : Bridges.MirrorNetworking.RoomServer
    {
        protected override ObservableServerProfile ProfileFactory(string username)
        {
            return new ObservableServerProfile(username)
            {
                new ObservableString((short)ObservablePropertiyCodes.DisplayName),
                new ObservableString((short)ObservablePropertiyCodes.Avatar),
                new ObservableInt((short)ObservablePropertiyCodes.Money),
                new ObservableFloat((short)ObservablePropertiyCodes.Health),
                new ObservableFloat((short)ObservablePropertiyCodes.Thirst),
                new ObservableFloat((short)ObservablePropertiyCodes.Hunger),
                new ObservableFloat((short)ObservablePropertiyCodes.Stamina),
                new ObservableInt((short)ObservablePropertiyCodes.ZombiesKilled),
                new ObservableInt((short)ObservablePropertiyCodes.PlayersKilled)
            };
        }
    }
}
