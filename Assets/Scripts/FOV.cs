using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FOV : MonoBehaviour
{
	[SerializeField] private float fov = 90f; // Field of view in degrees
	public float distance = 5f; // Max raycast distance
	[SerializeField] private int rayCount = 5; // Reduced for performance
	[SerializeField] private int smallerRaysCount = 6; // Reduced for performance
	[SerializeField] private float smallerRayDistance = 2f;
	[SerializeField] private AIPath aiPath;

	private Rigidbody2D rb;
	[SerializeField] private GameObject player;
	[SerializeField] private LayerMask layerMask;
	public bool canSeePlayer = false;
	private AILerp aiLerp;

	// Buffer array for RaycastNonAlloc results
	private RaycastHit2D[] hitsBuffer = new RaycastHit2D[20];

	// Detection timing
	private float checkInterval = 0.2f; // check every 0.2s (5x per second)
	private float checkTimer = 0f;

	void Start()
	{
		aiLerp = GetComponent<AILerp>();
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		aiPath = GetComponent<AIPath>();
	}

	void Update()
	{
		checkTimer -= Time.deltaTime;
		if (checkTimer <= 0f)
		{
			checkTimer = checkInterval;
			PerformDetection();
		}
	}

	private void PerformDetection()
	{
		if (rb == null || player == null || aiPath == null) return;

		float facingAngle = GetFacingAngle();

		// Reset vision flag
		canSeePlayer = false;

		if (CheckFOV(facingAngle)) return;
		CheckSmallerRays();
	}

	private float GetFacingAngle()
	{
		Vector2 nextWaypointDirection = ((Vector2)aiPath.steeringTarget - (Vector2)transform.position).normalized;
		return Mathf.Atan2(nextWaypointDirection.y, nextWaypointDirection.x) * Mathf.Rad2Deg;
	}

	private bool CheckFOV(float facingAngle)
	{
		float angleStep = fov / (rayCount - 1);
		float startAngle = facingAngle - (fov / 2f);

		for (int i = 0; i < rayCount; i++)
		{
			float angle = startAngle + (i * angleStep);
			Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

			int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, hitsBuffer, distance, layerMask);
			Debug.DrawRay(rb.position, direction * distance, Color.red);

			for (int j = 0; j < hitCount; j++)
			{
				if (hitsBuffer[j].collider != null && hitsBuffer[j].collider.gameObject == player)
				{
					canSeePlayer = true;
					return true;
				}
			}
		}

		return false;
	}

	private void CheckSmallerRays()
	{
		float angleIncrement = 360f / smallerRaysCount;

		for (int i = 0; i < smallerRaysCount; i++)
		{
			float angle = i * angleIncrement;
			Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

			int hitCount = Physics2D.RaycastNonAlloc(transform.position, direction, hitsBuffer, smallerRayDistance, layerMask);
			Debug.DrawRay(transform.position, direction * smallerRayDistance, Color.blue);

			for (int j = 0; j < hitCount; j++)
			{
				if (hitsBuffer[j].collider != null && hitsBuffer[j].collider.gameObject == player)
				{
					canSeePlayer = true;
					return;
				}
			}
		}
	}
}
