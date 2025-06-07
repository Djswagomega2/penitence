using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

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
		if (playerScript.hasDied && enemyTriggerScript.playerSteppedThrough)
		{
			StartCoroutine(ResetToOriginalPositions());
		}

		if (enemies.Count <= 0) 
		{
			Destroy(this);
		}

	
	}
	public IEnumerator ResetToOriginalPositions()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			GameObject enemy = enemies[i];
			Debug.Log("Gooning" + enemy);

			// Get components
			var fov = enemy.GetComponent<FOV>();
			var aiDestSetter = enemy.GetComponent<AIDestinationSetter>();
			var aiPath = enemy.GetComponent<AIPath>();
			StateManager stateManager = enemy.GetComponent<StateManager>();
<<<<<<< HEAD
			
=======
			State wanderState = enemy.transform.GetChild(1).transform.GetChild(0).GetComponent<WanderState>();
>>>>>>> origin/SkibidiDanielBranch

            // Fully disable chasing behavior
            if (fov != null) fov.enabled = false;
			if (aiDestSetter != null) aiDestSetter.target = null;
			if (aiPath != null) aiPath.enabled = false;
<<<<<<< HEAD
			if(stateManager != null) stateManager.currentState = enemy.transform.GetChild(1).transform.GetChild(0).GetComponent<WanderState>();// Set to idle or appropriate state
			Debug.Log("Disabling AI for enemy: " + stateManager.currentState);
=======
			if(stateManager != null)
			{
				stateManager.SwitchToTheNextState(wanderState); // Switch to Wander state
				stateManager.enabled = false; // Disable state manager to prevent state changes
			}
>>>>>>> origin/SkibidiDanielBranch


            // Reset position manually
            enemy.transform.position = originalPos[i];

			// Wait to ensure physics/AI updates don't interfere
			yield return new WaitForSeconds(0.1f);

			// Re-enable behavior
			if (aiPath != null) aiPath.enabled = true;
			if (fov != null) fov.enabled = true;
			if(stateManager != null) stateManager.enabled = true;
		}
	}

}
