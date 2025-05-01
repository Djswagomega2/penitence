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
	// Start is called before the first frame update
	void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
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
			//else say "you need a key to open this door"
			else
			{
				Debug.Log("You need a key to open this door.");
			}
		}
	}

}
