using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEnemy : MonoBehaviour
{
    [SerializeField]
    AudioSource walkAudio;
    public void WalkSound()
    {
        walkAudio.Play();
    }
}
