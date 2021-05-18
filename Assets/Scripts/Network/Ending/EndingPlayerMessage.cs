using UnityEditor;
using UnityEngine;
using Mirror;

public struct EndingPlayerMessage : NetworkMessage
{
    public string PlayerName;
    public PlayerEndingState endingState;
}