using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

interface IInteractable
{
    public void Interact();
}
public class InteractScript : MonoBehaviour
{
    public GameObject textHolder;
    public int interactRange;
    public Collider2D[] interactableList;
    public int selectedIndex;
    public LayerMask _layerMask;

    void Start()
    {
        selectedIndex = 0;
        interactRange = 2;
    }
    void Update()
    {
        interactableList = Physics2D.OverlapCircleAll(transform.position, interactRange, _layerMask);
        
        if (interactableList.Length > 0)
        {
			//textHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Press E to interact " + interactableList[selectedIndex].name;
			textHolder.SetActive(true);
			//handles more than 1 interactable
			if (interactableList.Length > 1)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, interactableList.Length-1);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    selectedIndex = Mathf.Clamp(selectedIndex - 1, 0, interactableList.Length-1);
                }
                //add text to display use of scrollwheel to change what u wanna interact with
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
               StartCoroutine(Interacting());
			}
		}
        else
        {
            textHolder.SetActive(false);

        }
    }

    private IEnumerator Interacting() 
    {
		textHolder.GetComponent<Image>().enabled = false;
		if (interactableList[selectedIndex].gameObject.TryGetComponent(out IInteractable interactGameObject))
		{
			interactGameObject.Interact();
			if (selectedIndex != 0) selectedIndex -= 1;
		}
        yield return new WaitForSeconds(0.5f);
		textHolder.GetComponent<Image>().enabled = true;
	}
}

