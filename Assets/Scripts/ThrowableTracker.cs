using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableTracker : MonoBehaviour
{

	public GameObject newProjectile;
	public float spawnerRadius;
	[SerializeField] private Collider2D[] enemies;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		enemies = Physics2D.OverlapCircleAll(newProjectile.transform.position, spawnerRadius, LayerMask.GetMask("Enemy"));
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
		foreach (Collider2D hit in enemies)
		{
			Pathfinding.AIDestinationSetter aiDestinationSetter = hit.GetComponent<Pathfinding.AIDestinationSetter>();
			if (aiDestinationSetter != null)
			{
				//Fix this 
				aiDestinationSetter.target = newProjectile.transform;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
	}
}
