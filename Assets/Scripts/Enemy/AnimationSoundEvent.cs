using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioClip[] playerAudioClip;
    [SerializeField] private Enemy[] enemy;
    private int enemyLength;
    private Transform[] enemyPos = new Transform[3];
    //범위 안에 있는지 확인하는 변수
    public bool isInArea;

    public void Awake()
    {
        enemy = FindObjectsOfType<Enemy>();
        enemyLength = enemy.Length;
        Debug.Log(enemyLength);
        for(int i=0; i<enemyLength; i++)
        {
            //enemyPos[i] = enemy[i].transform;
            enemy[i].AddAnimationSoundEvent(this);
        }        
    }
    public void MakeEventWalk()
    {
        ActiveSoundSensor();       
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
    public void SetInArea(bool tf) 
    {
        isInArea = tf;
    }
    public void ActiveSoundSensor()
    {
        playerAudioSource.Play();
        for (int i=0; i<enemyLength; i++)
        {
            if (enemy[i] && Vector3.Distance(transform.position, enemy[i].transform.position) <= 5f)
            {                                
                isInArea = (true);
                enemy[i].SoundSensorDetect();
            }            
        }        
    }
}
