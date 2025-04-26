using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public float amount;
    /*public AnimationClip healAnim;
    public AnimationClip reloadAnim;*/
	public ConsumableType consumableType;
    public enum ConsumableType
    {
        Health,
        Ammo
    }
    public override void MultiUse(PlayerScript caller, WeaponObjectScript weapCaller)
	{
        if (caller == null || weapCaller == null) 
        {
			Debug.LogError("Caller or WeaponObjectScript is null");
			return;
		} 
		switch (consumableType)
        {
            case ConsumableType.Health:
                caller.Heal(amount);
                break;
            case ConsumableType.Ammo:
                // Add ammo to the weapon
                weapCaller.AddAmmo((int)amount);
                break;
        }
		Debug.Log("use consumable");
    }
    public override ConsumableClass GetConsumable() { return this; }
}
