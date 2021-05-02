using UnityEditor;
using UnityEngine;
using Mirror;

public struct CreateRoomPlayerMessage : NetworkMessage
{
    public string name;
}

