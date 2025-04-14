using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectScript : MonoBehaviour
{
	//TODO: Fix ammo issue
	//TODO: Add Meleee
	//TODO: Make melee enum with bat and fist?
	//TOOD: Make different throwable class?
	//TODO: Fix Consumable objects
	#region Weapon Data
	[Header("Weapon Data")]
	public WeapClass WeapClassScript;
	public ScriptableObject item;
	public ScriptableObject fists;
	[SerializeField] private InventoryManager inventoryManager;
	//public Dictionary<string, AnimationClip> weaponAnimList;
	#endregion

	#region External Scripts
	[Header("External Scripts")]
	[SerializeField] private PlayerScript playerScript;
	[SerializeField] private Transform playerTransform;
	#endregion

	#region Weapon Variables
	[Header("Weapon Variables")]
	[SerializeField] private int ammo;
	[SerializeField] private GameObject weaponObject;
	[SerializeField] private GameObject fist;
	[SerializeField] private GameObject blockingObject;
	[SerializeField]  private GameObject projectile;
	[SerializeField] private float stunSeconds;
	[SerializeField] private float coolDownSeconds;
	#endregion

	#region Audio and SFX 
	[Header("Audio and SFX")]
	[SerializeField] private AudioSource weaponAudioSource;
	private ObjectPooler<AudioSource> gsPool;
	private ObjectPooler<AudioSource> gnsPool;
	private ObjectPooler<AudioSource> bcPool;
	#endregion

	#region Animation Variables
	[SerializeField] private Animator weaponAnimator;
	#endregion
	// Start is called before the first frame update
	void Start()
	{
		playerTransform = GetComponent<Transform>();
		playerScript = GetComponent<PlayerScript>();
		weaponAnimator = GetComponent<Animator>();
		inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
		gsPool = new ObjectPooler<AudioSource>(weaponAudioSource, ammo);
		gnsPool = new ObjectPooler<AudioSource>(weaponAudioSource, 20, null);
		bcPool = new ObjectPooler<AudioSource>(weaponAudioSource, ammo, null);
	}

	// Update is called once per frame
	void Update()
	{
		item = inventoryManager.selectedItem;
		if (item is WeapClass weap)
		{
			WeapClassScript = weap;
		}
		else
		{
			WeapClassScript = (WeapClass)fists;
		}

		ammo = WeapClassScript.ammoCapacity; //fix this to not be in void update, that's a future me problem...
		weaponAction();
	}

	public void weaponAction()
	{
		switch (WeapClassScript.weaponType)
		{
			case WeapClass.WeaponType.gun:
				Shoot();
				break;
			case WeapClass.WeaponType.melee:
				MeleeAttack();
				Block();
				break;
			case WeapClass.WeaponType.throwable:
				throwAction();
				break;
			default:
				Debug.Log("No weapon type selected");
				break;
		}
	}

	#region Shooting Methods
	public void Shoot()
	{
		//Add cool down
		if (Input.GetButtonDown("Fire1") && InventoryManager.isInventoryOpened == false)
		{
			if (ammo > 0)
			{
				StartCoroutine(playerScript.Shake());
				playerScript.muzzleflash.intensity = 50f;
				PlayGunShot();
				RaycastHit2D hit = Physics2D.Raycast(playerScript.firePoint.position, (Vector2)playerScript.mouseWorldPosition - (Vector2)playerScript.firePoint.position);
				if (hit)
				{
					if (hit.collider.gameObject.CompareTag("Enemy"))
					{
						StartCoroutine(stun(hit.collider.gameObject.GetComponent<Pathfinding.AILerp>(), stunSeconds));
					}
				}
				StartCoroutine(bulletShellSound());
				ammo--;
				playerScript.ammoBar.setAmmo(ammo);
			}
			else
			{
				PlayNoAmmo();
			}
		}
		playerScript.muzzleflash.intensity -= 2f;
		playerScript.muzzleflash.intensity = Mathf.Clamp(playerScript.muzzleflash.intensity, 0f, 50f); //not the hardcoded muzzle flash
	}

	private IEnumerator stun(Pathfinding.AILerp aiLerp, float stunTime)
	{
		aiLerp.canMove = false;
		yield return new WaitForSeconds(stunTime);
		aiLerp.canMove = true;
	}

	public void PlayGunShot()
	{
		AudioSource audioSource = gsPool.Get(transform.position, Quaternion.identity);
		audioSource.clip = WeapClassScript.attackSound; // Ensure the correct sound is assigned
		audioSource.Play();

		StartCoroutine(ReturnToGunShotPool(audioSource, audioSource.clip.length)); // Return after sound finishes
	}

	public void PlayNoAmmo()
	{
		AudioSource audioSource = gnsPool.Get(transform.position, Quaternion.identity);
		audioSource.clip = WeapClassScript.emptySound; // Ensure the correct sound is assigned
		audioSource.Play();

		StartCoroutine(ReturnToGunNoAmmoPool(audioSource, audioSource.clip.length)); // Return after sound finishes
	}

	public IEnumerator bulletShellSound()
	{
		yield return new WaitForSeconds(0.25f);
		AudioSource audioSource = bcPool.Get(transform.position, Quaternion.identity);
		audioSource.clip = WeapClassScript.reloadSound;
		audioSource.Play();
		StartCoroutine(ReturnToBulletCasePool(audioSource, audioSource.clip.length));

	}
	IEnumerator ReturnToGunShotPool(AudioSource source, float delay)
	{
		yield return new WaitForSeconds(delay);
		gsPool.ReturnToPool(source);
	}

	IEnumerator ReturnToGunNoAmmoPool(AudioSource source, float delay)
	{
		yield return new WaitForSeconds(delay);
		gnsPool.ReturnToPool(source);
	}
	IEnumerator ReturnToBulletCasePool(AudioSource source, float delay)
	{
		yield return new WaitForSeconds(delay);
		bcPool.ReturnToPool(source);
	}
	#endregion

	#region Melee Methods
	public void MeleeAttack()
	{
		if (Input.GetButtonDown("Fire1") && InventoryManager.isInventoryOpened == false)
		{
			/*if (WeapClassScript == fists)
			{

			}
			else
			{
				
			}*/ //<-- Prelim when we eventually have animation


			StartCoroutine(playWeaponAnim(null));

		}
	}
	public void Block()
	{
		if (Input.GetButtonDown("Fire2") && InventoryManager.isInventoryOpened == false)
		{
			StartCoroutine(playBlockAnim(null));
		}
	}


	private IEnumerator playWeaponAnim(string animName)
	{
		bool isPlayingAnim = true;//used to yield the time to the anim so it doesnt instantly destroy the anim object.
		Vector2 weaponPos = new Vector2(playerTransform.position.x + WeapClassScript.offsetVector.x, playerTransform.position.y + WeapClassScript.offsetVector.y);
		if (WeapClassScript == fists)
		{
			Instantiate(fist, weaponPos, Quaternion.Euler(0, 0, playerTransform.rotation.eulerAngles.z));
		}
		else
		{
			Instantiate(weaponObject, weaponPos, Quaternion.Euler(0, 0, playerTransform.rotation.eulerAngles.z));
		}
		if (isPlayingAnim)
		{
			weaponAnimator.Play(animName);
			isPlayingAnim = false;
			yield break;
		}
		yield return new WaitForSeconds(coolDownSeconds);
	}

	private IEnumerator playBlockAnim(string animName)
	{
		bool isPlayingAnim = true;//used to yield the time to the anim so it doesnt instantly destroy the anim object.
		Vector2 weaponPos = new Vector2(playerTransform.position.x + WeapClassScript.offsetVector.x, playerTransform.position.y + WeapClassScript.offsetVector.y);
		Instantiate(blockingObject, weaponPos, Quaternion.Euler(0, 0, playerTransform.rotation.eulerAngles.z));
		if (isPlayingAnim)
		{
			WeapClassScript.isBlocking = true;
			weaponAnimator.Play(animName);
			isPlayingAnim = false;
			yield break;
		}
		yield return new WaitForSeconds(1f);
		Destroy(blockingObject);
		WeapClassScript.isBlocking = false; 
		//Have blocking also hit back projectiles

	}
	#endregion

	#region throwable Methods
	private void throwAction()
	{
		if (Input.GetButtonDown("Fire1") && InventoryManager.isInventoryOpened == false)
		{
			if (WeapClassScript.throwableType == WeapClass.ThrowType.bottle)
			{
				throwBottle();
			}
			else if (WeapClassScript.throwableType == WeapClass.ThrowType.rock)
			{
				throwRock();
			}
		}
	}


	private void throwBottle()
	{
		//Makee this it's own function?
		Vector2 direction = ((Vector2)playerScript.mouseWorldPosition - (Vector2)playerScript.firePoint.position).normalized;
		GameObject newProjectile = Instantiate(projectile, playerScript.firePoint.position, Quaternion.identity);
		SpriteRenderer projectileSprite = newProjectile.GetComponent<SpriteRenderer>();
		projectileSprite.sprite = WeapClassScript.itemIcon;

		//Make this a function for creating bottles and other attack throwables?
		newProjectile.tag = "PlayerProjectile";
		BatMelee damage = newProjectile.AddComponent<BatMelee>();
		damage.weaponObjectScript = this;
		Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();
		if (projectileRb != null)
		{
			projectileRb.velocity = direction * WeapClassScript.throwSpeed;
		}
	}

	private void throwRock() 
	{
		Vector2 direction = ((Vector2)playerScript.mouseWorldPosition - (Vector2)playerScript.firePoint.position).normalized;
		GameObject newProjectile = Instantiate(projectile, playerScript.firePoint.position, Quaternion.identity);
		SpriteRenderer projectileSprite = newProjectile.GetComponent<SpriteRenderer>();
		projectileSprite.sprite = WeapClassScript.itemIcon;


		Rigidbody2D projectileRb = newProjectile.GetComponent<Rigidbody2D>();

		if (projectileRb != null)
		{
			projectileRb.velocity = direction * WeapClassScript.throwSpeed;
		}
		ObjectDestory objectDestory = newProjectile.GetComponent<ObjectDestory>();
		Destroy(objectDestory);

		//Imma be honest, this shit can probably be it's own script <-- again, future me figure this out
		float spawnerRadius = 2f;
		Collider2D[] enemies = Physics2D.OverlapCircleAll(newProjectile.transform.position, spawnerRadius, LayerMask.GetMask("Enemy"));
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
		foreach (Collider2D hit in enemies)
		{
			Pathfinding.AIDestinationSetter aiDestinationSetter = hit.GetComponent<Pathfinding.AIDestinationSetter>();
			if (aiDestinationSetter != null)
			{
				aiDestinationSetter.target = newProjectile.transform;
			}
		}

	}
	#endregion


}
