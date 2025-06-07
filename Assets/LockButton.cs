using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockButton : MonoBehaviour
{
    public GameObject doorButtonCode;
    public TextMeshProUGUI buttonText;
    public int buttonValue;
    public AudioSource audioSource;
    public AudioClip buttonSound;
	// Start is called before the first frame update
	void Start()
    {
        doorButtonCode = gameObject;
		audioSource = GetComponent<AudioSource>();
		buttonText = doorButtonCode.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		buttonText.text = buttonValue.ToString();
       
	}

    // Update is called once per frame
    void Update()
    {
        if(buttonValue > 9)
		{
			buttonValue = 0;
		}
		buttonText.text = buttonValue.ToString();
	}

    public void increaseValue() 
    {
        buttonValue++;
        audioSource.PlayOneShot(buttonSound);
	}
}
