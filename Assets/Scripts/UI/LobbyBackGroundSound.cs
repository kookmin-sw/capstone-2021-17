using UnityEditor;
using UnityEngine;


public class LobbyBackGroundSound : MonoBehaviour
{
    private void Awake()
    {
        if(SoundManager.LobbyBackGroundMusicObject == null)
        {
            DontDestroyOnLoad(this);
            SoundManager.LobbyBackGroundMusicObject = gameObject;

        }
        else
        {
            Destroy(this);
        }

        
    }
}
