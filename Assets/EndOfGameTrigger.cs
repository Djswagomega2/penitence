using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameTrigger : MonoBehaviour,IInteractable
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject dialougeBox;
    [SerializeField] private DialogueScript dialogueScript;
    [SerializeField] private float delay;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    IEnumerator EndOfGame() 
    {
        dialougeBox.SetActive(true);
        dialogueScript.dialogue("Lily...", delay);
        //yield return dialogueScript.DisplayText("Lily...", delay);
        yield return new WaitForSeconds(0.5f);
        gameManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Interact()
    {
       StartCoroutine(EndOfGame());
    }
}
