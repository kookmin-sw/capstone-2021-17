using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] 
    AudioSource AudioSource;
    public AudioClip[] sound;
    bool isCrouch; 
    Animator PlayerAnimator;
    bool check=false;


    void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CrouchCheck();
    }

    void PlayAudio(AudioClip audioclip,float f)
    {
        AudioSource.PlayOneShot(audioclip,f);
    }

    void StopAudio()
    {
        AudioSource.Stop();
    }

    void Crouch()
    {
        if (isCrouch&&check==false)
        {   
            PlayAudio(sound[1],0.1f);
            check=true;
        }
        else if (isCrouch)
        {   
            PlayAudio(sound[0],0.1f);
        }
    }

    void CrouchCheck()
    {
        isCrouch=PlayerAnimator.GetBool("Crouch");
        if(isCrouch==false)
        {
            StopAudio();
            check=false;
        }
    }
}
