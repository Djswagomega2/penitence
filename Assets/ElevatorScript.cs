using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour, IInteractable
{
	[SerializeField] private GameObject elevatorButtonPrefab;
	[SerializeField] private Animator elevatorAnimator;
	public int currentfloor; 


	// Start is called before the first frame update
	void Start()
    {
		elevatorButtonPrefab.SetActive(false);
	}

	public void Interact()
	{
		elevatorButtonPrefab.SetActive(true);
	}

	public void changeFloor(int floor)
	{
		elevatorButtonPrefab.SetActive(false);
		currentfloor = floor;
		StartCoroutine(elevatorShake());
    }

	IEnumerator elevatorShake() 
	{
		elevatorAnimator.SetTrigger("ElevatorShake");
		yield return new WaitForSeconds(1f);
		elevatorAnimator.SetTrigger("ElevatorStill");
    }
}
