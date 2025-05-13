using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerScript : MonoBehaviour,IDamageable
{
    //Make Level 3 One floor
    //Fix the lighting
    #region General Variables
    [Header("General")]
    public float health;
    private Rigidbody2D rb;
    private CircleCollider2D playerCol;
    [SerializeField] private Animator johnAnimator;
	#endregion

	#region Movement Variables
	[Header("Movement")]
	[SerializeField] private float speed;
	[SerializeField] private float sprintSpeed;
	[SerializeField] private float defaultSpeed;
	[SerializeField] private float speedMultiplyer;
	[SerializeField] private KeyCode[] sprintButtons;
    [SerializeField] private float stamina; 
	private float hor;
    private float vert;
    private Vector2 dir;
    private Vector3 velocity = Vector3.zero;
	#endregion

	#region Camera Variables
	[Header("Camera")]
	private Camera _cam;
    public Vector3 mouseWorldPosition;
    private float lookAngle;
    public float smooth = 0.5f;
    public AnimationCurve curve;
    public float duration = 1f;
	#endregion

	#region Flashlight Variables
	[Header("Flashlight")]
	[SerializeField] private GameObject flashlight;
	[SerializeField] private bool isFlashlightOn;
	#endregion

	#region Attacking Variables
	[Header("Attacking")]
	[SerializeField] public Transform firePoint;
    public Transform muzzle;
	#endregion

	#region Respawning Variables
	[Header("Respawning")]
    [SerializeField] private GameObject spawner;
    [SerializeField] private LayerMask spawnerMask;
    [SerializeField] private int spawnerRadius;
    public GameObject droplet;
	#endregion

	#region UI Variables
	[Header("UI")]
    [SerializeField] public InventoryManager inventory;
	#endregion

	#region Light2D Variables
	[Header("Light2D")]
	public Light2D muzzleflash;
    [SerializeField] private Light2D[] flashlightLights;
	#endregion

	//[SerializeField] private Sprite normalJohn;
	//[SerializeField] private Sprite hasGun;

	//private SpriteRenderer sr;


	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CircleCollider2D>();
		_cam = Camera.main;
        InstantiateDroplet(this.transform.position);
        muzzleflash = muzzle.GetComponent<Light2D>();
        flashlightLights[0] = flashlight.GetComponent<Light2D>();
        flashlightLights[1] = flashlight.transform.GetChild(0).GetComponent<Light2D>();
        johnAnimator = GetComponent<Animator>();
		//health = 100f;
        speed = defaultSpeed;
        sprintSpeed = defaultSpeed * speedMultiplyer; //These can be changed

	}

    #region Update Methods
    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        dir = new Vector2(hor, vert).normalized;

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle - 90f, Vector3.forward);

        if (Input.GetKey(sprintButtons[0]) || Input.GetKey(sprintButtons[1]))
        {
            StartCoroutine(dashing());
		}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            flashlight.SetActive(!flashlight.activeSelf);
            isFlashlightOn = !isFlashlightOn;
		}
        
        InventoryHandler();
        RespawnParse();
        Respawn();
        InstantiateDroplet(this.transform.position);
		//FlashlightDecrease();
	}
    private void LateUpdate()
    {
        CameraHandler();
    }
    void FixedUpdate()
    {
        rb.velocity = dir * speed * Time.deltaTime;
    }
    #endregion

	#region Movement Methods
    private IEnumerator dashing()
	{
		speed = sprintSpeed;
		yield return new WaitForSeconds(stamina);
		speed = defaultSpeed;
	}
	#endregion

	#region Flashlight Methods
    private void FlashlightDecrease()
	{
		if (isFlashlightOn)
		{ 
			for (int i = 0; i < flashlightLights.Length; i++)
			{
				flashlightLights[i].intensity -= Time.deltaTime/10;

				if (flashlightLights[i].intensity <= 0)
				{
					flashlightLights[i].intensity = 0;
					flashlight.SetActive(false);
					isFlashlightOn = false;
				}
			}
		}
		
	}
	#endregion

	#region Camera Methods
	private void CameraHandler()
    {
        float magnitude = 2f;
        float xMidpoint = Mathf.Clamp((mouseWorldPosition.x - transform.position.x) / 2, -magnitude, magnitude);
        float yMidpoint = Mathf.Clamp((mouseWorldPosition.y - transform.position.y) / 2, -magnitude, magnitude);

        _cam.transform.position = Vector3.SmoothDamp(_cam.transform.position, new Vector3(transform.position.x + xMidpoint, transform.position.y + yMidpoint, -1f), ref velocity, smooth);

    }
    public IEnumerator Shake()
    {
        Vector2 startPosition = (Vector2)_cam.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float mag = curve.Evaluate(elapsedTime / duration);
            Vector2 offset = Random.insideUnitCircle * mag;
            _cam.transform.position = new Vector3(startPosition.x + offset.x, startPosition.y + offset.y, _cam.transform.position.z);
            yield return null;
        }

        _cam.transform.position = new Vector3(startPosition.x, startPosition.y, _cam.transform.position.z);
    }
	#endregion

	#region Inventory Methods
	private void InventoryHandler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventory.selectedItem != null)
                inventory.selectedItem.Use(this);
        }
    }
	#endregion

	#region Respawn Methods
	void RespawnParse()
    {
        Collider2D[] circleCols = Physics2D.OverlapCircleAll(this.transform.position, spawnerRadius, spawnerMask);
		for (int i = 0; i < circleCols.Length; i++)
		{
            Collider2D circleCol = circleCols[i];
			if (circleCol == spawner || circleCol == null)
			{
                continue; 
			}

            spawner = circleCol.gameObject;
            break;
		}
    }
    //Down the line change this an IEnumator where it waits for the Taste/Death Animation to finish before Respawning
    void Respawn()
    {
        if(health <= 0)
        {
			this.transform.position = spawner.transform.position;
            health = 100;
        }
    }
	#endregion

	#region Health Methods
	public void UpdateHealth(float newHealthValue)
    {
        health = newHealthValue;
    }
   public void ReceiveDamage(float damage)
   {
        var updatedHealth = health - damage;
        UpdateHealth(updatedHealth > 0 ? updatedHealth : 0);
        //StartCoroutine(Invincablity());
   }

	public void Heal(float healAmount)
	{
		var updatedHealth = health + healAmount;
        johnAnimator.Play("John_Heal");
		UpdateHealth(updatedHealth < 100 ? updatedHealth : 100);
	}

	private IEnumerator Invincablity() 
    {
        playerCol.enabled = false;
        Debug.Log("Player is invincible for 1 second");
        yield return new WaitForSeconds(1f);
        playerCol.enabled = true;
    }

    private IEnumerator puddleDamage() 
    {
        ReceiveDamage(0.5f);
		yield return new WaitForSeconds(2f);
	}
	#endregion;

	#region Player Tracking Methods
	private GameObject InstantiateDroplet(Vector2 position)
    {
        if (droplet != null)
        {
            Destroy(droplet);
        }
        droplet = new GameObject("Droplet");
        droplet.transform.position = position;
        CircleCollider2D cirCollider = droplet.AddComponent<CircleCollider2D>(); // Add a collider to the point
        cirCollider.isTrigger = true; // Set collider as trigger
        droplet.tag = "Droplet";
        droplet.layer = LayerMask.NameToLayer("Droplet"); // Set the layer to Droplet
        return droplet;
    }
	#endregion

	#region Collision Methods
	private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
			case "Enemy":
				Enemy enemy = collision.gameObject.GetComponent<Enemy>();
				ReceiveDamage(enemy.EnemyDmg);
				break;
			case "Projectile":
                ReceiveDamage(10);
				break;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Puddle"))
		{
			StartCoroutine(puddleDamage());
		}
	}

	#endregion

	private void OnDrawGizmos()
    {
		Gizmos.DrawWireSphere(this.transform.position, spawnerRadius);
    }

    
}