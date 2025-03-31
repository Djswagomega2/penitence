using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class disgustingFuckingFlashlightScript : WeapClass
{
    public Ray2D flashRayShooter; //the ray being used
    public RaycastHit2D flashRayShooterInformation;
    [SerializeField ]public KeyCode bind; //for shitty keybinding idk what you want man
    public GameObject player;
    public LayerMask collisionLayer;
    public InventoryManager inventoryIsOpenChecker; //I dont know why I put this here

    //nums for actual flashing
    public float maxRange; //for editing from unity
    public float flashArc; //arc of flash from camera
    public int rayCount; //num of rays from flash
    //idk maybe add arcIncriment 

    // Start is called before the first frame update
    void Start()
    {
        bind = KeyCode.Mouse0;
        maxRange = 10f; //default
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// camera flash, it fucking sucks but who cares I need to refactor for angle shots per request
    /// </summary>
    public void Flash()
    {
        bool hasHit;
        for (int i = 10; i == 0; i--)
        {

            flashRayShooterInformation = Physics2D.Raycast(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), maxRange, collisionLayer);
            //Debug.DrawRay
        }

        if (flashRayShooterInformation)
        {
            Debug.Log(flashRayShooterInformation.collider.gameObject.name);
            hasHit = true; //its to make sure that you dont register multiple rays hitting an enemy doing more damage
        }

        //if (flashRayShooterInformation.collider.gameObject.tag == "Enemy" && )
        //{
        //    flashRayShooterInformation.collider.gameObject.GetComponent<Enemy>().ReceiveDamage(30);
        //}
    }
}

*/