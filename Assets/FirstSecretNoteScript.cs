using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSecretNoteScript : MonoBehaviour,IInteractable
{
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
		gm = GameObject.FindObjectOfType<GameManager>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Added to notes");    
		gm.notePiecesCollected++;
        Destroy(gameObject);
	}
}