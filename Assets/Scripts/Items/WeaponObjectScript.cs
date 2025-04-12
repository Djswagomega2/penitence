using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectScript : MonoBehaviour
{
	public WeapClass WeapClassScript;
	public ScriptableObject item;
	[SerializeField] private InventoryManager inventoryManager;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		item = inventoryManager.selectedItem;
		if (item is WeapClass weap)
		{
			WeapClassScript = weap;
		}
		else
		{
			WeapClassScript = null;
		}
		weaponAction();
	}

	public void weaponAction() 
	{
		switch (WeapClassScript.weaponType) 
		{
			case WeapClass.WeaponType.gun:
				if(Input.GetButtonDown("Fire1") && InventoryManager.isInventoryOpened == false)
				{
					if (WeapClassScript.playerScript.ammo > 0)
					{
						WeapClassScript.Shoot(); //this doesn't work we might have to import this into the script
					}
					else
					{
						Debug.Log("Out of ammo");
					}
				}
				break;
			case WeapClass.WeaponType.melee:
				//melee action
				break;
			case WeapClass.WeaponType.throwable:
				//throwable action
				break;
			default:
				Debug.Log("No weapon type selected");
				break;
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			collision.gameObject.GetComponent<Enemy>().ReceiveDamage(WeapClassScript.weaponDamage);
		}
	}

}
