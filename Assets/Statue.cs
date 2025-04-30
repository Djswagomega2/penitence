using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : MonoBehaviour, IInteractable
{
    //Resize Statue 
    //Change dialouge to be array based 

    public Queue<string> textQueue = new Queue<string>();
    public enum StatueType{Statue,EvilStatue}
    public StatueType statueType;
    public DialogueScript dialogueScript;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image statueImage;
    [SerializeField] private GameObject staute;
	[SerializeField] private GameObject dialougeBox;
	[SerializeField] private Sprite[] statueCloseUps;
	[SerializeField] private Collider2D interactCollider;
    [SerializeField] private List<string[]> statueDialogues = new List<string[]>();
    [SerializeField] private int dialougeIndex; // Index to track which statue dialogue to use

	private bool isTalking = false;

    void Start()
    {
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        dialougeBox.GetComponentInChildren<Image>().sprite = null;
		// Only setup multiple dialogues for regular Statue
		if (statueType == StatueType.Statue)
		{
			statueDialogues.Add(new string[] {"I am a statue.","I have stood here for centuries.","My purpose is unknown."});
			statueDialogues.Add(new string[] {"Time flows differently for me.","Still as the stone I’m carved from."});
			statueDialogues.Add(new string[] {"This world changes.","But I remain."});
		}

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
        //fix this queue is being cleared 
		textQueue.Clear(); 

		if (statueType == StatueType.Statue)
		{
			if (dialougeIndex < statueDialogues.Count)
			{
				foreach (string line in statueDialogues[dialougeIndex])
				{
					textQueue.Enqueue(line);
				}
			}
			else
			{
				// Optional fallback if dialogueIndex exceeds defined dialogues
				textQueue.Enqueue("I have nothing more to say.");
			}

			statueImage.sprite = statueCloseUps[0];
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


		if (statueType == StatueType.Statue)
		{
			statueImage.sprite = statueCloseUps[0];
		}
		else if (statueType == StatueType.EvilStatue)
		{
			statueImage.sprite = statueCloseUps[1];
		}
	}


    private IEnumerator PlayDialogue()
    {
        isTalking = true;
        statueImage.enabled = true;
        dialougeBox.SetActive(true);

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
			dialougeIndex++;
			yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            interactCollider.enabled = false; // Disable the collider after interaction
            staute.layer = LayerMask.NameToLayer("Default"); //fix this past the first statue


        }
        else if (statueType == StatueType.EvilStatue)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            GameObject.Find("Player").GetComponent<PlayerScript>().ReceiveDamage(999);
            interactCollider.enabled = false; // Disable the collider after interaction
            staute.layer = LayerMask.NameToLayer("Default");
        }

        dialougeBox.SetActive(false);
		statueImage.enabled = false;
        isTalking = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueScript.textDisplay.text = "";
    }
}
