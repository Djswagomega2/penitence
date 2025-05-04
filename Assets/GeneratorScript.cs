using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour,IInteractable
{
    public LevelOneManager levelOneManager;
    public GameObject generator;
    public Sprite activeGenerator;
    public AudioSource audioSource;
    public GameObject trainLights;
    public GameObject trainNoise;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        trainLights.SetActive(false);
        trainNoise.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        levelOneManager.generatorAmountTurnedOn++;
        generator.layer = LayerMask.NameToLayer("Default");
        generator.GetComponent<SpriteRenderer>().sprite = activeGenerator;
        audioSource.Play();
        trainLights.SetActive(true);
        trainNoise.SetActive(true);
    }
}
