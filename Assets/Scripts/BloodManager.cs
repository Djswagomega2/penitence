using UnityEngine;

public class BloodManager : MonoBehaviour
{
	public BloodScript bloodPrefab; // Prefab reference
	private ObjectPooler<BloodScript> bloodPool; // Pool instance

	private void Start()
	{
		if (bloodPrefab == null)
		{
			Debug.LogError("BloodManager: bloodPrefab is NULL! Make sure to assign it in the Inspector.");
			return;
		}

		bloodPool = new ObjectPooler<BloodScript>(bloodPrefab, 20, transform);

		// Make sure all objects in the pool are initialized before use
		for (int i = 0; i < 20; i++)
		{
			BloodScript blood = bloodPool.Get();
			blood.SetPool(bloodPool); // Important!
			bloodPool.ReturnToPool(blood);
		}
	}

	public void SpawnBlood(Vector3 position, Quaternion rotation)
	{
		if (bloodPool == null)
		{
			Debug.LogError("BloodManager: bloodPool is NULL when trying to spawn blood!");
			return;
		}

		BloodScript blood = bloodPool.Get(position, rotation);
		blood.SetPool(bloodPool); // Ensure the blood knows which pool it belongs to
	}
}
