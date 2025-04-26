using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour, IInteractable
{
    //Resize Statue 
    //Add a decidated dialouge box
    //Fix the head not popping up
    //Change dialouge to be array based 
    //Add new evil statue

    public Queue<string> textQueue = new Queue<string>();
    public enum StatueType{Statue,EvilStatue}
    public StatueType statueType;
    public DialogueScript dialogueScript;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image statueImage;
    [SerializeField] private GameObject staute;
    [SerializeField] private Collider2D interactCollider;

    private bool isTalking = false;

    void Start()
    {
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    public void Interact()
    {
        if (!isTalking)
        {
            PopulateTextQueue();
            StartCoroutine(PlayDialogue());
        }
    }

    private void PopulateTextQueue()
    {
        if (textQueue.Count == 0)
        {
            if (statueType == StatueType.Statue)
            {
                textQueue.Enqueue(string.Empty);
                textQueue.Enqueue("I am a statue.");
                textQueue.Enqueue(string.Empty); // <- This was added to ensure the text queue is not empty
                textQueue.Enqueue("I have stood here for centuries.");
                textQueue.Enqueue("My purpose is unknown.");
            }
            else if (statueType == StatueType.EvilStatue)
            {
                textQueue.Enqueue(string.Empty);
                textQueue.Enqueue("Why do you");
                textQueue.Enqueue(string.Empty);
                textQueue.Enqueue("Keep making the same mistakes...");
                textQueue.Enqueue(string.Empty); // <- This was added to ensure the text queue is not empty
                textQueue.Enqueue("OVER");
                textQueue.Enqueue("AND OVER AGAIN!");
            }
        }
    }

    private IEnumerator PlayDialogue()
    {
        isTalking = true;
        statueImage.enabled = true;

        while (textQueue.Count > 0)
        {
            string text = textQueue.Dequeue();
            dialogueScript.dialogue(text, 0.01f);

            // Wait for player to press a key to continue
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E)); // <- customize input if you want
        }

        if (statueType == StatueType.Statue)
        {
            gameManager.statuesInteracted++;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            interactCollider.enabled = false; // Disable the collider after interaction
            staute.layer = LayerMask.NameToLayer("Default");


        }
        else if (statueType == StatueType.EvilStatue)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            GameObject.Find("Player").GetComponent<PlayerScript>().ReceiveDamage(999);
            interactCollider.enabled = false; // Disable the collider after interaction
            staute.layer = LayerMask.NameToLayer("Default");
        }

        statueImage.enabled = false;
        isTalking = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueScript.textDisplay.text = "";
    }
}
