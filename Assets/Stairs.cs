using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] private GameObject player;
	[SerializeField] private Animator transition;
	[SerializeField] private Transform floorPlace;
	[SerializeField] private GameObject crossfadeObject;
	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Player"))
		{
			StartCoroutine(TransitionToFloor());
		}
	}

	private IEnumerator TransitionToFloor()
	{
		player.transform.position = floorPlace.position;
		crossfadeObject.SetActive(true);
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(1f);
		transition.Play("Crossfade_End");
		crossfadeObject.SetActive(false);
	}
}
