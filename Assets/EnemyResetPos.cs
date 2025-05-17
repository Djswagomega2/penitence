using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyResetPos : MonoBehaviour
{
	[SerializeField] private PlayerScript playerScript;
	[SerializeField] private EnemyTriggerScript enemyTriggerScript;
	public List<GameObject> enemies;
	public List<Vector3> originalPos;

	void Start()
	{
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

		for (int i = 0; i < enemies.Count; i++)
		{
			originalPos[i] = enemies[i].transform.position;
		}
	}

	void Update()
	{
		if (playerScript.health <= 0 /*<-- this might be happening way to fast*/ && enemyTriggerScript.playerSteppedThrough)
		{
			Debug.Log("Damn you suck");
			ResetToOriginalPositions();
		}

		if (enemies.Count <= 0) 
		{
			Destroy(this);
		}

	
	}
	public void ResetToOriginalPositions()
	{
		Debug.Log("Resetting enemy positions...");
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i] != null)
			{
				Debug.Log($"Resetting {enemies[i].name} to {originalPos[i]}");
				enemies[i].transform.position = originalPos[i];
			}
		}
	}
}
