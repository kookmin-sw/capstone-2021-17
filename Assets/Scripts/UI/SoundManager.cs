using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static GameObject LobbyBackGroundMusicObject;

    public AudioSource[] audioSource;
    public Slider[] volume_slider;



    //0번 : 배경음악
    //1번 : 효과음(버튼 등)
    //2번 : 캐릭터 & 에너미 음

    private void Awake()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GamePlay")
        {
            if (LobbyBackGroundMusicObject) Destroy(LobbyBackGroundMusicObject);
        }
    }
    //음악 재생
    public void PlaySound(int num)
    {;
        audioSource[num].Play();
    }

//음량 조절
    //배경음악
    public void SetMusicVolume(float volume)
    {
        audioSource[0].volume = volume;
    }
    //효과음(버튼)
    public void SetEffectVolume(float volume)
    {
        audioSource[1].volume = volume;
    }
    //캐릭터음(걷는 소리, 타격소리, 적 소리 등등)
    public void SetCharacterVolume(float volume)
    {
        audioSource[2].volume = volume;
    }

    public void AudioFadeOut(AudioSource audioSoruce, float FadeTime)
    {
        IEnumerator fadeSound = FadeOut(audioSoruce, FadeTime);
        StartCoroutine(fadeSound);
        StopCoroutine(fadeSound);
    }

    public void FadeOutLobbyBackGroundMusic()
    {
        if (LobbyBackGroundMusicObject)
        {
            AudioSource audioSource = LobbyBackGroundMusicObject.GetComponent<AudioSource>();
            AudioFadeOut(audioSource, 0.5f);
        }   
    }

    private IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}
