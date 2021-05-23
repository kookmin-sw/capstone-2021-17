using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingPlayerManager : MonoBehaviour
{//엔딩씬의 캐릭터 움직임 제어
    private Animator ending_anim;
    public TMP_Text Nickname_text;
    public bool isLocalPlayer;
    
    private AudioSource audioSource;
    
    Vector3 target;

    void Start()
    {
        ending_anim.SetBool("isStop", false);
    }

    void Awake()
    {
        ending_anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isLocalPlayer = false;
    }

    public void Stop()
    {
        ending_anim.SetBool("isStop", true);
        StopAudio();
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
