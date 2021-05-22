using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Teleporter : MonoBehaviour
{



    ParticleSystem[] particleSystems;

    public List<ParticleSystem.MainModule> ParticleMains;

    public GameObject Flare;
    // Start is called before the first frame update
    void Awake()
    {

        particleSystems = GetComponentsInChildren<ParticleSystem>();
        ParticleMains = new List<ParticleSystem.MainModule>();

        //Debug.Log(renderers.Length);


        foreach(var p in particleSystems)
        {
            var main = p.main;
            ParticleMains.Add(main);

            Color color = main.startColor.color;


            main.startColor = new Color(color.r, color.g, color.b, 0);
;       }

    }

    EndingPlayerManager endingPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            endingPlayer = other.GetComponent<EndingPlayerManager>();
            StartCoroutine("WarpPlayer");

        }
    }

    IEnumerator WarpPlayer()
    {
        endingPlayer.Stop();
        
        Flare.SetActive(true);

        if (endingPlayer.isLocalPlayer)
        {
            AudioSource warpSound = Flare.GetComponent<AudioSource>();
            warpSound.Play();
        }
        

        yield return new WaitForSeconds(1);
        SkinnedMeshRenderer[] renderers = endingPlayer.GetComponentsInChildren<SkinnedMeshRenderer>();
        Color color = renderers[0].material.color;

        FadeIn(1);

        string playerName = endingPlayer.Nickname_text.text;

        endingPlayer.Nickname_text.gameObject.SetActive(false);

        float wtime = 0;
        while (renderers[0].material.color.a > 0f)
        {
            wtime += Time.deltaTime / 1;

            float alpha = Mathf.Lerp(1, 0, wtime);
            foreach(var r in renderers)
            {
                r.material.SetColor("_Color", new Color(color.r, color.g, color.b, alpha));
            }

            yield return null;
        }

        EndingManager.instance.EndingNames.Add(playerName);
        EndingManager.instance.EndingStates.Add(PlayerEndingState.Escape);

        if (endingPlayer.isLocalPlayer)
        {
            yield return new WaitForSeconds(1);

            EndingManager.instance.ShowEndUI();
        }
        
    }

    bool isPlaying = false;
    float time;
    float fadeTime;

    public void FadeOut(float FadeTime)

    {

        if (isPlaying == true) //중복재생방지

        {

            return;

        }

        this.fadeTime = FadeTime;

        StartCoroutine("fadeoutplay");

    }

    IEnumerator fadeoutplay()
    {

        isPlaying = true;

        time = 0f;

        while (ParticleMains[0].startColor.color.a < 1f)

        {
            time += Time.deltaTime / fadeTime;

            float alpha = Mathf.Lerp(0, 1, time);

            foreach ( var p in particleSystems)
            {
                var main = p.main;
                Color c = main.startColor.color;
                main.startColor = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;

        }

        isPlaying = false;
    }

    public void FadeIn(float FadeTime)
    {

        if (isPlaying == true) 

        {

            return;

        }

        this.fadeTime = FadeTime;

        StartCoroutine("fadeinplay");

    }

    IEnumerator fadeinplay()

    {

        isPlaying = true;

        time = 0f;

        float startAlpha = particleSystems[0].main.startColor.color.a;

        while (particleSystems[0].main.startColor.color.a > 0f)

        {
            time += Time.deltaTime / fadeTime;

            float alpha = Mathf.Lerp(startAlpha, 0, time);

            foreach (var p in particleSystems)
            {
                var main = p.main;
                Color c = main.startColor.color;
                main.startColor = new Color(c.r, c.g, c.b, alpha);
            }

            yield return null;

        }

        isPlaying = false;
    }




}
