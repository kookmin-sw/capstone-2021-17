using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundEnemy : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;   //����� �ҽ�
    [SerializeField] private AudioClip[] clips;         //������ Ŭ����
    [SerializeField] private BoxCollider boxCollider;   //�ȿ� �޸� �ݶ��̴���    
    void Start()
    {        
        SetAttCollider();  //�ݶ��̴� ����
    }

    //�ȴ� �Ҹ� ���
    public void WalkSound()
    {
        audioSource.PlayOneShot(clips[0], 1f);
    }

    //���� �ÿ� �Ҹ� ���
    public void AttackSound()
    {
        audioSource.PlayOneShot(clips[1], 0.5f);
    }

    //���� ����, ���� �ݶ��̴� �Ѱ� ����
    public void SetAttCollider()
    {
        boxCollider.enabled = boxCollider.enabled ? false : true;
    }

    //���������ϴ� ���� ���
    public void DizzySound()
    {
        audioSource.PlayOneShot(clips[2], 0.7f);
    }
}
