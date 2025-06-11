using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSpawn : MonoBehaviour
{
	[SerializeField] private Transform[] floorPlace;
	[SerializeField] private GameObject player;
	[SerializeField] private Animator transition;
	[SerializeField] private GameObject crossfadeObject;
	[SerializeField] private ElevatorScript elevatorScript;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(TransitionToFloor());
		}
	}
	private IEnumerator TransitionToFloor()
	{
		player.transform.position = floorPlace[elevatorScript.currentfloor].position;
		crossfadeObject.SetActive(true);
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(1f);
		transition.Play("Crossfade_End");
		crossfadeObject.SetActive(false);
	}
}
