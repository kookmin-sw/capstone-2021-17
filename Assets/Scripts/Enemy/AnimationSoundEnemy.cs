using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;   //오디오 소스
    [SerializeField] private AudioClip[] clips;         //실행할 클립들
    [SerializeField] private BoxCollider boxCollider;   //팔에 달린 콜라이더들    
    void Start()
    {        
        SetAttCollider();  //콜라이더 오프
    }

    //걷는 소리 출력
    public void WalkSound()
    {
        audioSource.PlayOneShot(clips[0], 1f);
    }

    //공격 시에 소리 출력
    public void AttackSound()
    {
        audioSource.PlayOneShot(clips[1], 0.5f);
    }

    //공격 시작, 끝에 콜라이더 켜고 끄기
    public void SetAttCollider()
    {
        boxCollider.enabled = boxCollider.enabled ? false : true;
    }

    //어지러워하는 사운드 출력
    public void DizzySound()
    {
        audioSource.PlayOneShot(clips[2], 0.7f);
    }
}
