using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] ambiences;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = Random.Range(30, 60);
    }
    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            audioSource.clip = ambiences[Random.Range(0, ambiences.Length-1)];
            audioSource.Play();
            timer = Random.Range(30f, 60f);
        }
    }
}
