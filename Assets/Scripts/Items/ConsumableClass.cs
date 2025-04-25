using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public float restoreHealth;

    public override void Use(PlayerScript caller)
    {
        base.Use(caller);
        caller.Heal(restoreHealth);
		Debug.Log("use consumable");
        //caller.inventory.UsedSelected();
    }
    public override ConsumableClass GetConsumable() { return this; }
}
