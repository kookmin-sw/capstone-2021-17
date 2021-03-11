using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxContoller : MonoBehaviour
{
    private Animation boxAnim;
    private void Awake()
    {
        boxAnim = gameObject.GetComponent<Animation>();
    }

    public void PlayAnimation()
    {
        boxAnim.Play("Crate_Open");
    }
}
