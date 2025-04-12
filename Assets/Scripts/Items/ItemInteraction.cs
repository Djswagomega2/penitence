using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour, IInteractable
{
    public ItemClass item;
    public InventoryManager inventory;
    public void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = item.itemIcon;
        this.name = item.itemName;
        inventory = GameObject.FindObjectOfType<InventoryManager>();
    }
    public void Interact()
    {
        inventory.Add(item,1);
        Destroy(gameObject);
    }
}
