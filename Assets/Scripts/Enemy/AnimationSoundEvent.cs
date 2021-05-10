using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] playerAudioClip;
    private Enemy[] enemy;
    private int enemyLength;
    private Transform[] enemyPos = new Transform[3];
    //범위 안에 있는지 확인하는 변수
    private bool isInArea = false;

    public void Start()
    {
        enemy = FindObjectsOfType<Enemy>();
        enemyLength = enemy.Length;
        for(int i=0; i<enemyLength; i++)
        {
            enemyPos[i] = enemy[i].transform;
        }        
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
        for(int i=0; i<enemyLength; i++)
        {
            if (Vector3.Distance(transform.position, enemyPos[i].position) <= 5f)
            {
                enemy[i].SoundSensorDetect();
                isInArea = true;
            }
            else
            {
                isInArea = false;
            }
        }
    }
}
