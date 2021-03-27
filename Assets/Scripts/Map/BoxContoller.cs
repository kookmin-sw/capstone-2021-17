using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxContoller : MonoBehaviour
{
    private Animation boxAnim;
    private ItemBoxNetBehaviour boxNetBehaviour;
    private void Awake()
    {
        boxAnim = gameObject.GetComponent<Animation>();
        boxNetBehaviour = GetComponent<ItemBoxNetBehaviour>();
    }


    public void OpenBox()
    {
        boxNetBehaviour.OpenBox();
    }

    public void PlayAnimation()
    {
        boxAnim.Play("Crate_Open");
    }
}
