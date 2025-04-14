using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
	[SerializeField] private Gradient ammoGradient;
	[SerializeField] private Image fill;
	[SerializeField] private Image currentWeapon;
	[SerializeField] private Sprite fist;
	[SerializeField] private ScriptableObject item;
	[SerializeField] private InventoryManager inventoryManager;

	private void Start()
	{
		slider = GetComponent<Slider>();
	}

	public void setMaxAmmo(int ammo)
	{
		slider.maxValue = ammo;
		slider.value = ammo;
		fill.color = ammoGradient.Evaluate(1f);
	}
	public void setAmmo(int ammo)
	{
		slider.value = ammo;
		fill.color = ammoGradient.Evaluate(slider.normalizedValue);
	}

	public void Update()
	{
		item = inventoryManager.selectedItem;
		if (item is WeapClass weap)
		{
			currentWeapon.sprite = weap.itemIcon;
/*			setMaxAmmo(weap.ammoCapacity);
			setAmmo(weap.ammoCapacity);*/
		}
		else
		{
			currentWeapon.sprite = fist;	
			/*setMaxAmmo(0);
			setAmmo(0);*/
		}
	}

}
