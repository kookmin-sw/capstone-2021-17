using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] sound;
    private AudioSource[] audioSource;
    public Slider[] volume_slider;

    //0번 : 배경음악
    //1번 : 효과음(버튼 등)
    //2번 : 캐릭터 & 에너미 음

    void Awake()
    {
        for(int i=0; i<audioSource.Length; i++)
        {
            this.audioSource[i] = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

//음악 재생
    public void PlaySound(int num)
    {
        audioSource[num].clip = sound[num];
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

}
