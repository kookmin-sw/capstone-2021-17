using UnityEngine;
using System.Collections;

public class LeverController : MonoBehaviour
{
    public string playerOrder;
    public string leverOrder;

    public int pullLimit;
    public int pulls;

    public AudioClip pullSound;
    public AudioClip resetSound;
    public AudioClip enableSound;
    private AudioSource mainAudio; //Private declaration to the audiosource component

    public GameObject[] interactiveObjects;

    public GameObject readyBtn;
    public GameObject resettingBtn;
    public GameObject acceptedBtn;
    public GameObject limitReachedBtn;

    private bool resetting;
    public bool open = false;

    void Start()
    {
        mainAudio = GetComponent<AudioSource>(); //Finds the AudioSource on the GameObject
    }

    void Update()
    {
        if (pulls >= pullLimit)
        {
            readyBtn.GetComponent<Renderer>().material.color = Color.red;
            limitReachedBtn.GetComponent<Renderer>().material.color = Color.green;
            open = true;
        }
    }

    public void LeverReset()
    {
        pulls = 0;
        playerOrder = "";
        ResetSound();

        StartCoroutine(Timer(1.0f));
        if (resetting)
        {
            readyBtn.GetComponent<Renderer>().material.color = Color.red;
            resettingBtn.GetComponent<Renderer>().material.color = Color.green;
            acceptedBtn.GetComponent<Renderer>().material.color = Color.red;
            limitReachedBtn.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    public void LeverCheck()
    {
        if (playerOrder == leverOrder)
        {
            pulls = 0;
            EnableSound();

            LeverInteraction();

            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                interactiveObjects[i].gameObject.tag = "Untagged";
            }

            readyBtn.GetComponent<Renderer>().material.color = Color.red;
            resettingBtn.GetComponent<Renderer>().material.color = Color.red;
            acceptedBtn.GetComponent<Renderer>().material.color = Color.green;
            limitReachedBtn.GetComponent<Renderer>().material.color = Color.red;
        }

        else
        {
            LeverReset();
        }
    }

    void LeverInteraction()
    {
        //IF THE CODE IS CORRECT - DO SOMETHING HERE!
    }

    public void PullSound()
    {
        mainAudio.PlayOneShot(pullSound, 1.0f); //Play beep, at a volume of 0.2
    }

    public void ResetSound()
    {
        mainAudio.PlayOneShot(resetSound, 1.0f);
    }

    public void EnableSound()
    {
        mainAudio.PlayOneShot(enableSound, 1.0f);
    }

    IEnumerator Timer(float waitTime)
    {
        resetting = true;
        yield return new WaitForSeconds(waitTime);
        resettingBtn.GetComponent<Renderer>().material.color = Color.red;
        readyBtn.GetComponent<Renderer>().material.color = Color.green;
        resetting = false;
    }    
}
