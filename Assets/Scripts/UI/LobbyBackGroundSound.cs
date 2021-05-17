using UnityEditor;
using UnityEngine;


public class LobbyBackGroundSound : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        SoundManager.LobbyBackGroundMusicObject = gameObject;
    }
}
