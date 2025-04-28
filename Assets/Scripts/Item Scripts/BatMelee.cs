using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMelee : MonoBehaviour
{
	public WeaponObjectScript weaponObjectScript;

	private void Start()
	{
		weaponObjectScript = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponObjectScript>();
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Debug.Log("I've been hit");
			collision.gameObject.GetComponent<Enemy>().ReceiveDamage(weaponObjectScript.WeapClassScript.weaponDamage);
		}
	}
}
