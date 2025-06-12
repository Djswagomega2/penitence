using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour, IInteractable
{
	[SerializeField] private GameObject elevatorButtonPrefab;
	[SerializeField] private CameraShake cameraShake;
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
	}

}
