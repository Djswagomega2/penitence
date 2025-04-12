using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssignItemToObjectScript : MonoBehaviour
{
    public ItemClass Item;
    public bool canBeInteracted;
    public GameObject textHolder;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = Item.itemIcon;
        this.name = Item.itemName;
        textHolder.SetActive(false);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            

        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        textHolder.SetActive(false);
    }
}