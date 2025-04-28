using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponObjectScript : MonoBehaviour
{
	//TODO: Fix ammo issue
	    //Make damage trigger based
    //Fix position of the weapon to be based of the mouse click for melee
	#region Weapon Data
	[Header("Weapon Data")]
	public WeapClass WeapClassScript;
	public ScriptableObject item;
	public ScriptableObject lastEquippedItem;
	private Dictionary<ScriptableObject, int> weaponAmmoDict = new Dictionary<ScriptableObject, int>();
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
	[SerializeField] private bool ammoInitialized;
	[SerializeField] private GameObject weaponObject;
	[SerializeField] private GameObject fist;
	[SerializeField] private GameObject blockingObject;
	[SerializeField]  private GameObject projectile;
	[SerializeField] private float stunSeconds; //Can potentially be moved to weap class
	[SerializeField] private float coolDownSeconds; //Can potentially be moved to weap class
	#endregion

	#region Audio and SFX 
	[Header("Audio and SFX")]
	[SerializeField] private AudioSource weaponAudioSource;
	private ObjectPooler<AudioSource> gsPool;
	private ObjectPooler<AudioSource> gnsPool;
	private ObjectPooler<AudioSource> bcPool;
	#endregion

	#region UI
	[Header("Weapon Variables")]
	public AmmoBar ammoBar;
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
		WeapClassScript = (WeapClass)fists;
	}

	// Update is called once per frame
	void Update()
	{
		item = inventoryManager.selectedItem;

		if (item != lastEquippedItem)
		{
			if (item is WeapClass weap)
			{
				WeapClassScript = weap;
				ammoBar.currentWeapon.sprite = WeapClassScript.itemIcon;

				// If we haven't tracked ammo for this weapon yet, initialize it
				if (!weaponAmmoDict.ContainsKey(item))
				{
					weaponAmmoDict[item] = weap.ammoCapacity;
				}
				ammo = weaponAmmoDict[item];
				ammoBar.setMaxAmmo(weap.ammoCapacity);
				ammoBar.setAmmo(ammo);
			}
			else if (item is ConsumableClass consumable) 
			{
				ammoBar.currentWeapon.sprite = WeapClassScript.itemIcon;
				ammoBar.setAmmo(ammo);
			}
			else
			{
				WeapClassScript = (WeapClass)fists;
				ammoBar.currentWeapon.sprite = WeapClassScript.itemIcon;
				ammo = 0;
				ammoBar.setMaxAmmo(0);
				ammoBar.setAmmo(0);
			}

			if (item != lastEquippedItem)
			{
				RefreshAmmo();
				lastEquippedItem = item;
			}
		}

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
				weaponAmmoDict[item] = ammo; // <-- persist the ammo change
				ammoBar.setAmmo(ammo);
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

	private void RefreshAmmo()
	{
		if (item == null) return;

		if (!weaponAmmoDict.ContainsKey(item))
		{
			weaponAmmoDict.Add(item, 0);
		}

		ammo = weaponAmmoDict[item];
		ammoBar.setAmmo(ammo);
	}

	public void AddAmmo(int additionalAmmo)
	{
		if (item == null) return;

		ammo += additionalAmmo;

		if (weaponAmmoDict.ContainsKey(item))
		{
			weaponAmmoDict[item] = ammo;
		}
		else
		{
			weaponAmmoDict.Add(item, ammo);
		}

		// IMMEDIATELY update the ammo bar to match the new value
		ammoBar.setAmmo(ammo);

		// OPTIONAL: RefreshAmmo() if needed
		// RefreshAmmo(); <-- only if your ammoBar is funky

		Debug.Log("Ammo added: " + additionalAmmo + ". New ammo count: " + ammo);
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
		Vector2 weaponPos = new Vector2(playerTransform.position.x + WeapClassScript.offsetVector.x, playerTransform.position.y + WeapClassScript.offsetVector.y); // fix this to be more accurate 
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

	#region Throwable Methods
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

		ThrowableTracker tracker = newProjectile.AddComponent<ThrowableTracker>();
		tracker.newProjectile = newProjectile;
		tracker.spawnerRadius = 5f;

	}

	#endregion


}
