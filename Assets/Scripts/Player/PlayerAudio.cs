using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] 
    AudioSource AudioSource;
    public AudioClip[] Sound;
    bool isCrouch; 
    bool isJump;
    Animator PlayerAnimator;
    bool Check=false;
    public bool Checks=false;
    public float jump;


    void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        CrouchCheck();
        JumpCheck();
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
        if (isCrouch&&Check==false)
        {   
            PlayAudio(Sound[1],0.1f);
            Check=true;
        }
        else if (isCrouch)
        {   
            PlayAudio(Sound[0],0.1f);
        }
    }

    void CrouchCheck()
    {
        isCrouch=PlayerAnimator.GetBool("Crouch");
        if(isCrouch==false)
        {
            StopAudio();
            Check=false;
        }
    }

    void JumpCheck()
    {
        bool isjump=PlayerAnimator.GetBool("OnGround");
        if(isjump==false&&Checks==false)
        {
            PlayAudio(Sound[2],0.1f);
            Checks=true;

        }
        else if(isjump)
        {
            Checks=false;
        }
    }
}
