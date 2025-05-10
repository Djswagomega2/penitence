using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThrowableTracker : MonoBehaviour
{

	public GameObject newProjectile;
	public float spawnerRadius;
	[SerializeField] private Collider2D[] enemies;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private bool hasStopped;
	public Pathfinding.AIDestinationSetter aiDestinationSetter;
	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		TrackEnemies();
		StartCoroutine(ThrowingTime(5f));
	}

    // Update is called once per frame
    void Update()
    {

		if (hasStopped) 
		{
			StartCoroutine(destoryRock());
		}
	}

	private void TrackEnemies() 
	{
		enemies = Physics2D.OverlapCircleAll(newProjectile.transform.position, spawnerRadius, LayerMask.GetMask("Enemy"));
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
		foreach (Collider2D hit in enemies)
		{
			aiDestinationSetter = hit.GetComponentInParent<Pathfinding.AIDestinationSetter>();
			Debug.Log("Hit: " + hit.name);
			if (aiDestinationSetter != null)
			{
				aiDestinationSetter.target = newProjectile.transform;
			}
		}
	}

	private IEnumerator ThrowingTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		rb.velocity = Vector2.zero;
		hasStopped = true;

	}

	private IEnumerator destoryRock() 
	{
		yield return new WaitForSeconds(5f);
		Destroy(newProjectile);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Obstacle"))
		{
			rb.velocity = Vector2.zero;
			hasStopped = true;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
	}
}
