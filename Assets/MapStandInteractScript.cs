using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapStandInteractScript : MonoBehaviour, IInteractable
{
	[SerializeField] private Image mapImage;
	// Start is called before the first frame update
	void Start()
    {
		mapImage.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
       
    }
	public void Interact()
	{
		mapImage.enabled = true;
	}
}
