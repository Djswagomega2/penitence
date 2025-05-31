using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DoorLockScript : MonoBehaviour, IInteractable
{
	[SerializeField] private GameObject doorCode;
	[SerializeField][Range(0f, 9f)] private int[] buttonCombo;
	[SerializeField] private LockButton[] buttonCodes;
	public bool hasInteracted;
	public bool codeHasBeenSolved;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (hasInteracted)
		{
			Time.timeScale = 0;
			doorCode.SetActive(true);
			//player.GetComponent<PlayerScript>().enabled = false;
		}
		else
		{
			Time.timeScale = 1;
			doorCode.SetActive(false);
		}

		if(Input.GetKeyDown(KeyCode.Escape) && hasInteracted)
		{
			hasInteracted = false;
			doorCode.SetActive(false);
			Time.timeScale = 1;
			//player.GetComponent<PlayerScript>().enabled = true;
		}

		if (buttonCodes[0].buttonValue == buttonCombo[0] &&
			buttonCodes[1].buttonValue == buttonCombo[1] &&
			buttonCodes[2].buttonValue == buttonCombo[2])
		{
			codeHasBeenSolved = true;
		}

		if (codeHasBeenSolved)
		{
			hasInteracted = false;
			doorCode.SetActive(false);
			Destroy(gameObject);
			Debug.Log("Door code solved! Door can now be opened.");
			// You can add more functionality here, like opening the door or playing a sound.
		}
	}

	public void Interact()
	{
		hasInteracted = !hasInteracted;
	}
}
