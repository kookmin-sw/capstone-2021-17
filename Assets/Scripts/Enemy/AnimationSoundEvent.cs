using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] playerAudioClip;
    [SerializeField] private Enemy enemy;

    private Transform enemyPos;
    //범위 안에 있는지 확인하는 변수
    private bool isInArea = false;

    public void Awake()
    {
        enemyPos = enemy.transform;
    }
    public void MakeEventWalk()
    {
        ActiveSoundSensor();
        playerAudioSource.Play();
    }

    public void MakeEventCrouch()
    {        
        playerAudioSource.PlayOneShot(playerAudioClip[0],0.1f);
    }

    public void WalkSound()
    {
        playerAudioSource.Play();
    }

    public bool CheckInArea()
    {
        return isInArea;
    }

    public void ActiveSoundSensor()
    {
        if (Vector3.Distance(transform.position, enemyPos.position) <= 5f)
        {
            enemy.SoundSensorDetect();
            isInArea = true;
        }
        else
        {
            isInArea = false;
        }
    }
}
