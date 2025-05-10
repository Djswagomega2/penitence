using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour,IInteractable
{
    public InventoryManager inventoryManager;
    private ScriptableObject item;
	[SerializeField] private bool isClosed = true;
	[SerializeField] private AudioSource doorSource;
	[SerializeField] private AudioClip doorOpenSound;
	[SerializeField] private DialogueScript dialogueScript;
    [SerializeField] private GameObject dalougeBox;
	private bool isShowingDialogue = false;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
		doorSource = GetComponent<AudioSource>();
		isClosed = true;
	}

    // Update is called once per frame
    void Update()
    {
        if(!isClosed)
		{
			// Open the door
			// Add your door opening logic here
			Destroy(gameObject);
			doorSource.clip = doorOpenSound;
			doorSource.Play();

		}
	}

	public void Interact()
	{
		item = inventoryManager.selectedItem;
		if (item is MiscClass misc)
		{
			if (misc.miscType == MiscClass.MiscType.Key)
			{
				isClosed = false;
			}
		}
        else
        {
			if (!isShowingDialogue)
			{
				StartCoroutine(lockedDialouge());
			}
        }
    }

	private IEnumerator lockedDialouge() 
	{
        isShowingDialogue = true;
        dalougeBox.SetActive(true);
		dialogueScript.dialogue("The door is locked", 0.01f);
        yield return new WaitForSeconds(0.01f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        dalougeBox.SetActive(false);
        isShowingDialogue = false;
    }

}
