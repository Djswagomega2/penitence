using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionScript : MonoBehaviour
{
    //the idea for this script is there being two invisible collision boxes (triggers) and once they fire a button prompt pops up and the interaction is available

    public GameObject interactionButton; //button to load when the two 
    public GameObject parentObject; //what object this button is attached to. I know its not optimized since you have a bunch of scripts but idk I dont care Ill optimize later
    public Vector3 offsetVector;
    public float fromEditorOffsetMod = 10f;
    public bool isTextInteractable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Input.GetKey(KeyCode.Mouse0) && gameObject.tag == "interactable" && collision.gameObject.tag == "player") //btw there is probably a better check for interactable, who cares
        {
            offsetVector = parentObject.transform.position;
            offsetVector.x += fromEditorOffsetMod;
            Instantiate(interactionButton, offsetVector, Quaternion.identity);
        }


    }
    public void showText()
    {
        if (isTextInteractable)
        {

        }
    }

    public void pickUpItem() //this exists just in case idk why we need this
    {

    }

    //feel free to add anything here idk what kind of event system you want so deal as you wish
}

