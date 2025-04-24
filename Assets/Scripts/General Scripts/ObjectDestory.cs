using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestory : MonoBehaviour
{
	// Start is called before the first frame update[
	public float destorySeconds;

	/*void Start()
    {
        StartCoroutine(DestroyObject());
	}*/

	private void OnEnable()
	{
		StartCoroutine(DestroyObject());
	}

	private IEnumerator DestroyObject()
	{
		yield return new WaitForSeconds(destorySeconds);
		Destroy(gameObject);
	}
}
