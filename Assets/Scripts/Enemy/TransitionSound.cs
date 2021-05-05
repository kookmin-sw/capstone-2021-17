using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSound : StateMachineBehaviour
{
    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip landClip;
    AudioSource audioSource;
    AnimationSoundEvent animationSoundEvent;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);        
        audioSource.PlayOneShot(jumpClip, 1f);
        animationSoundEvent.ActiveSoundSensor();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        audioSource.PlayOneShot(landClip, 1f);
        animationSoundEvent.ActiveSoundSensor();
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    public void SetAnimationEvnet(AnimationSoundEvent animationSoundEvent)
    {
        this.animationSoundEvent = animationSoundEvent;
    }
}
