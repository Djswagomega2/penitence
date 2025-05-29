using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour,IDamageable
{
	//TODO: Find a way to pool less blood objects (maybe make it so the know how may enemies are on the stage, and divde it by their health?)
	//TODO: Make scriptable objects for enemyDamage,MaxHp, and potentially speed?
    //TODO: Camera does more damage, but doesn't stun. Bat does decent damage, but does stun. 

	public int MaxHp;

    public int EnemyDmg;

    public float _currentHealth;

    public GameObject spawner;

    public GameObject resetPos;

    public GameObject player;

    public PlayerScript playerScript;

    public Vector3 originalPos;

	[SerializeField] private GameObject bloodSpray;
    [SerializeField] private GameObject bloodDrop;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip hurtSound;
    private ObjectPooler<GameObject> bloodDropPool;
    private ObjectPooler<GameObject> hurtSoundPool;
    private ObjectPooler<GameObject> bloodSprayPool;

    [SerializeField] private Pathfinding.AILerp aiLerp;
    [SerializeField] private Pathfinding.AIPath aiPath;
    

    public void Start()
    {
        _currentHealth = MaxHp;
        _audioSource = GetComponentInChildren<AudioSource>();
		bloodDropPool = new ObjectPooler<GameObject>(bloodDrop,10,null);
        hurtSoundPool = new ObjectPooler<GameObject>(_audioSource.gameObject,5,null);
		//bloodSprayPool = new ObjectPooler<GameObject>(bloodSpray,20,null);
		aiLerp = GetComponent<AILerp>();
        aiPath = GetComponent<AIPath>();
		originalPos = transform.position; // Store the original position of the enemy
		player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();

		if (spawner == null) 
        {
            Debug.Log("This enemy did not come from a spawner");
        }

	}

    public void Update()
    {
        Death();
    }
    public void UpdateHealth(float newHealthValue)
    {
        _currentHealth = newHealthValue;
    }

    public void ReceiveDamage(float damage)
    {
        var updatedHealth = _currentHealth - damage;
        UpdateHealth(updatedHealth > 0 ? updatedHealth : 0);
        DropBlood(5, 1f);
        PlayHurtSound();

    }
   public void DropBlood(int amount, float spread)
   {
        for (int i = 0; i < amount; i++)
        {
            GameObject blood = bloodDropPool.Get((Vector2)(transform.position + Random.insideUnitSphere * spread), Quaternion.identity);
        }
   }

    public void Death() 
    {
		if (_currentHealth <= 0)
		{
			Destroy(gameObject);

			int index = resetPos.GetComponent<EnemyResetPos>().enemies.IndexOf(gameObject);
			if (index != -1)
			{
				resetPos.GetComponent<EnemyResetPos>().enemies.RemoveAt(index);
				resetPos.GetComponent<EnemyResetPos>().originalPos.RemoveAt(index);
			}

			if (spawner != null) 
            {
			    spawner.GetComponent<Spawner>().spawnedEnemies.Remove(gameObject);
			}
		}
	}

    /*private void SprayBlood()
    {
        GameObject bloodSprayObj = bloodSprayPool.Get(transform.position,Quaternion.identity);
        ParticleSystem particleSystem = bloodSprayObj.GetComponent<ParticleSystem>();
        particleSystem.Play();
        StartCoroutine(ReturnBloodSprayToPool(bloodSprayObj,2.1f));
    }*/

    private void PlayHurtSound()
    {
        GameObject soundObj = hurtSoundPool.Get(transform.position, Quaternion.identity); // Get GameObject
        AudioSource audioSource = soundObj.GetComponent<AudioSource>(); // Get AudioSource component
        audioSource.clip = hurtSound; // Ensure the correct sound is assigned
        audioSource.Play();
        
        StartCoroutine(ReturnSoundToPool(soundObj, audioSource.clip.length)); // Return after sound finishes
    }

    public IEnumerator ReturnBloodToPool(GameObject blood, float delay)
    {
        yield return new WaitForSeconds(delay);
        bloodDropPool.ReturnToPool(blood);
    }

  /*  private IEnumerator ReturnBloodSprayToPool(GameObject bloodSpray, float delay)
    {
        yield return new WaitForSeconds(delay);
        bloodDropPool.ReturnToPool(bloodSpray);
    }*/

   private IEnumerator ReturnSoundToPool(GameObject soundObj, float delay)
   {
        yield return new WaitForSeconds(delay);
        soundObj.SetActive(false);
        hurtSoundPool.ReturnToPool(soundObj);
   }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Projectile")) 
        {
			ReceiveDamage(5);
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Blocking"))
		{
			aiLerp.canMove = false;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Blocking"))
		{
			aiLerp.canMove = false;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Blocking"))
		{
			aiLerp.canMove = true;
		}
	}

} 
