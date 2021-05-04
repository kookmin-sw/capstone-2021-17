using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] 
    AudioSource audioSource;
    public AudioClip[] sound;    
    bool isCrouch; 
    bool isJump;
    Animator playerAnimator;
    bool check=false;
    public bool checks=false;
    public float jump;


    void Awake()
    {
        playerAnimator = GetComponent<Animator>();        
    }

    void Update()
    {
        PlayTest();
        CrouchCheck();
        JumpCheck();
    }

    void PlayAudio(AudioClip audioclip,float f)
    {
        audioSource.PlayOneShot(audioclip,f);
    }

    void StopAudio()
    {
        audioSource.Stop();
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
        isCrouch=playerAnimator.GetBool("Crouch");
        if(isCrouch==false)
        {
            StopAudio();
            check=false;
        }
    }

    void JumpCheck()
    {
        bool isjump=playerAnimator.GetBool("OnGround");
        if(isjump==false&&checks==false)
        {
            PlayAudio(sound[2],0.1f);
            checks=true;

        }
        else if(isjump)
        {
            checks=false;
        }
    }
    
    //어떤 클립 재생하는지 확인
    public void Blank()
    {
        for(int i=0; i<playerAnimator.GetCurrentAnimatorClipInfo(0).Length; i++)
        {
            Debug.Log(playerAnimator.GetCurrentAnimatorClipInfo(0)[i].clip.name);
        } 
    }

    /*public void idleTest()
    {
        if(PlayerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name !="HumanoidIdle")
        {
            AudioSource.Play();
        }
        
    }*/

    //앉는 동안, 일어서는 동안 등 transition에서 소리나게 하기 
    //
    public void PlayTest()
    {
        if(playerAnimator.GetAnimatorTransitionInfo(0).IsName("Grounded -> Airborne"))
        {
            audioSource.Play();
        }
        else if (playerAnimator.GetAnimatorTransitionInfo(0).IsName("Airborne -> Grounded"))
        {
            audioSource.Play();
        }
        else if (playerAnimator.GetAnimatorTransitionInfo(0).IsName("Grounded -> Crouching"))
        {
            audioSource.Play();
        }
        else if (playerAnimator.GetAnimatorTransitionInfo(0).IsName("Crouching -> Grounded"))
        {
            audioSource.Play();
        }
    }

}
