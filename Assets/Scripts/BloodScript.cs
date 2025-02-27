using System.Collections;
using UnityEngine;

public class BloodScript : MonoBehaviour
{
	private Vector3 localScale;
	private Color newColor;
	private Color oldColor;
	private float alpha;
	private float size;
	private SpriteRenderer spriteRenderer;
	private float despawnTime;
	private ObjectPooler<BloodScript> pool;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetPool(ObjectPooler<BloodScript> poolReference)
	{
		pool = poolReference;
	}

	private void OnEnable()
	{
		// Reset properties every time it's taken from the pool
		newColor = spriteRenderer.color;
		oldColor = spriteRenderer.color;
		alpha = newColor.a;
		size = Random.Range(0.5f, 1.5f);
		transform.localScale = new Vector3(size, size, 1f);
		localScale = transform.localScale;

		despawnTime = Random.Range(2f, 5f); // Set random despawn time if needed
		StartCoroutine(FadeOutAndReturnToPool());
	}
	


	private IEnumerator FadeOutAndReturnToPool()
	{
		while (despawnTime > 0)
		{
			despawnTime -= Time.deltaTime;
			yield return null;
		}

		while (newColor.a > 0)
		{
			newColor.a -= 0.001f;
			spriteRenderer.color = Color.Lerp(oldColor, newColor, 1f);
			yield return null;
		}

		if (pool == null)
		{
			Debug.LogError("BloodScript: pool is NULL when trying to return to pool!");
			yield break; // Exit the coroutine to prevent errors
		}

		pool.ReturnToPool(this); // Return blood object to the pool <-- Causes an Error fix this please!S
	}

	
}
