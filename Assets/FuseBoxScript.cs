using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FuseBoxScript : MonoBehaviour, IInteractable
{
	public InventoryManager inventoryManager;
	private ScriptableObject item;
	[SerializeField] private DialogueScript dialogueScript;
	[SerializeField] private GameObject dalougeBox;
	[SerializeField] private bool isActivated;
	private bool isShowingDialogue = false;
	[SerializeField] private Sprite elevatorOpened;
	[SerializeField] private GameObject[] elevators;
	[SerializeField] private AudioSource fuseBoxSource;
	[SerializeField] private AudioClip fuseBoxOpenSound;

	// Start is called before the first frame update
	void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
		fuseBoxSource = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			isActivated = true;
		}
		if (isActivated) 
		{
			fuseBoxSource.clip = fuseBoxOpenSound;
			fuseBoxSource.Play();
			foreach (GameObject elevator in elevators)
			{
				elevator.GetComponent<SpriteRenderer>().sprite = elevatorOpened;
				elevator.GetComponent<BoxCollider2D>().isTrigger = true;
			}
		}
	}

	public void Interact()
	{
		foreach (SlotClass slot in inventoryManager.items)
		{
			if (slot.GetItem() is MiscClass misc) 
			{
				if (misc.miscType == MiscClass.MiscType.Key && slot.GetQuantity() >= 3)
				{
					isActivated = true;
				}
			}
		}

		if (!isActivated && !isShowingDialogue)
		{
			StartCoroutine(lockedDialouge());
		}
		else if (isActivated && !isShowingDialogue)
		{
			StartCoroutine(unlockedDialouge());
		}
	}
	private IEnumerator lockedDialouge()
	{
		isShowingDialogue = true;
		dalougeBox.SetActive(true);
		dialogueScript.dialogue("Hmmm, seems that I don't have all 3 fuses", 0.01f);
		yield return new WaitForSeconds(0.01f);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
		dalougeBox.SetActive(false);
		isShowingDialogue = false;
	}

	private IEnumerator unlockedDialouge()
	{
		isShowingDialogue = true;
		dalougeBox.SetActive(true);
		dialogueScript.dialogue("Seems like the elevator started working", 0.01f);
		yield return new WaitForSeconds(0.01f);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
		dalougeBox.SetActive(false);
		isShowingDialogue = false;
	}
}
