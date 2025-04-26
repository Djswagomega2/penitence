using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour,IInteractable
{
    public LevelOneManager levelOneManager;
    public GameObject generator;
    public Sprite activeGenerator;

    void Start()
    {
        
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
        Debug.Log("Turned this on");
    }
}
