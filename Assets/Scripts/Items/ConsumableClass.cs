using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public float amount;
    public ConsumableType consumableType;
    public enum ConsumableType
    {
        Health,
        Ammo
    }
    public override void Use(PlayerScript caller)
    {
        base.Use(caller);
        switch(consumableType)
        {
            case ConsumableType.Health:
                caller.Heal(amount);
                break;
            case ConsumableType.Ammo:
                // Add ammo to the weapon
                //AddAmmo();
                break;
        }
        caller.Heal(amount);
		Debug.Log("use consumable");
    }

    public void AddAmmo(WeaponObjectScript weapCaller)
    {
        weapCaller.AddAmmo((int)amount);

    }
    public override ConsumableClass GetConsumable() { return this; }
}
