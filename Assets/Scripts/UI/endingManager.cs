using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endingManager : MonoBehaviour
{
    private Animator ending_anim;
    public Transform character;
    Vector3 target;
    // Start is called before the first frame update
    void Awake()
    {
        ending_anim = GetComponent<Animator>();
        
    }

    void Start()
    {
        ending_anim.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isLive()
    {
        ending_anim.SetBool("isDead", false);
        target = new Vector3(0, 0, -6.4f);
        float start = 0;
        while(transform.position != target){
            start += Time.deltaTime * 0.1f;
            transform.position = Vector3.Lerp(transform.position, target, start);
        }
        Debug.Log("click live");
    }

    public void isDead()
    {
        target = new Vector3(0, -0.4f, -6.4f);
        float start = 0;
        ending_anim.SetBool("isDead", true);
        while(transform.position != target){
            start += Time.deltaTime * 0.1f;
            transform.position = Vector3.Lerp(transform.position, target, start);
        }
        Debug.Log("click dead");
    }
}
