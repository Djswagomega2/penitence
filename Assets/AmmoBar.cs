using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
	[SerializeField] private Gradient ammoGradient;
	[SerializeField] private Image fill;
	public Image currentWeapon;

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

}
