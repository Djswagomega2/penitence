using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
	[SerializeField] private GameObject floorOne;
	[SerializeField] private GameObject floorTwo;
	[SerializeField] private GameObject floorThree;

	private enum Floor
	{
		FloorOne,
		FloorTwo,
		FloorThree
	}

	private Floor floor;

	private void Start()
	{
		floorOne.SetActive(true);
		floorTwo.SetActive(false);
		floorThree.SetActive(false);
	}

	private void Update()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			floorOne.SetActive(false);
			floorTwo.SetActive(true);
			floorThree.SetActive(false);
		}
		else if(collision.gameObject.CompareTag("Player"))
		{
			floorOne.SetActive(false);
			floorTwo.SetActive(false);
			floorThree.SetActive(true);
		}
		else
		{
			floorOne.SetActive(true);
			floorTwo.SetActive(false);
			floorThree.SetActive(false);
		}
	}
}
