using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "new Weapon Class", menuName = "Item/Weapon")]
public class WeapClass : ItemClass
{
    //TODO; Switch over the Audio stuff to Object Pooling
    // old weapon shit (Nam)
    [Header("Weapon")]
    public WeaponType weaponType;
    public int weaponDamage;
    public int ammoCapacity;
    public float _fireRate;
    public float firingError;
    public float damageFalloffRange;
    public float damageFalloff;
    public float reloadSpeed;
    public AudioClip attackSound;
    public AudioClip reloadSound;

    //new weapon shit (Toto) \
    [Header("Shitter variables")]
    public Animation weaponAnimController; //controls the weapon anim
    public Dictionary<string, AnimationClip> weaponAnimList; //dict of anims to play from. First value is anim name, second value is anim clip.
    public float weaponDurability; //might be vestigial, have to recheck with team to see if this doesn't work out.
    public GameObject weaponObject; //physical representation of the weapon if necessary.
    public AudioSource weaponAudioSource; //There wasn't an audio source for the weapon so I decided to add it, kinda bloaty so might remove later. 
    public Transform gameObjectTransform; //sort of a bypass measure 
    public Vector3 offsetVector; //used to offset insantiation position of weaponobject.
    public float maxRange; //for editing from unity
    public Ray hitscanRay;
    public float blockDecimal;
    public bool isBlocking;

    //shit to make the ripped out code work
    public PlayerScript playerScript;


    //I just want to make something clear. The weapon object is separate from the weapon itself.
    //The weapon is an absrtact concept which controls the values of the weapon object.
    //This way, if we really wanna fuck around, we can make the changes without having to tear up too much infrastructure. 
    //Also, I am aware of itemobject. I don't know what it use cases that was intended for, so this will have to do. If bloat just remove and Ill rewrite. 
    //Also, I've only set it up for melee since I presume we're just doing hitscan since projectiles aren't really necessary.

    public enum WeaponType { gun, melee }//why tf do we have this

    public override void Use(PlayerScript caller)
    {
        base.Use(caller);
        Debug.Log("Attack");
    }
    public virtual void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && InventoryManager.isInventoryOpened == false)
        {
            if (playerScript.ammo > 0)
            {
                playerScript.StartCoroutine(playerScript.Shake());
                playerScript.muzzleflash.intensity = 50f;
                playerScript.PlayGunShot();
                RaycastHit2D hit = Physics2D.Raycast(playerScript.firePoint.position, (Vector2)playerScript.mouseWorldPosition - (Vector2)playerScript.firePoint.position);
                if (hit)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<Enemy>().ReceiveDamage(playerScript.enemyDamage);
                    }
                }
                playerScript.StartCoroutine(playerScript.bulletShellSound());
                playerScript.ammo--;
            }
            else
            {
                playerScript.PlayNoAmmo();
            }
        
        //yes, I did just rip this from the player script, and no I am not fixing it.
        //Why the fuck was everything private. 
        }
        
    }
    //public override WeapClass GetWeap() { return this; }

    public virtual IEnumerator playWeaponAnim(string animName)
    {
        bool isPlayingAnim = true; ; //used to yield the time to the anim so it doesnt instantly destroy the anim object.
        offsetVector.x = weaponObject.transform.parent.position.x;
        offsetVector.y = weaponObject.transform.parent.position.y;
        offsetVector.z = weaponObject.transform.parent.position.z;
        gameObjectTransform.position = offsetVector;
        Instantiate(weaponObject, gameObjectTransform);  
        if (isPlayingAnim)
        {
            weaponAnimController.Play(animName);
            isPlayingAnim = false;
            yield break;
        }
        Destroy(weaponObject);
        yield return null;
    }

    public virtual IEnumerator playBlockAnim(string animName)
    {
        bool isPlayingAnim = true; ; //used to yield the time to the anim so it doesnt instantly destroy the anim object.
        offsetVector.x = weaponObject.transform.parent.position.x;
        offsetVector.y = weaponObject.transform.parent.position.y;
        offsetVector.z = weaponObject.transform.parent.position.z;
        gameObjectTransform.position = offsetVector;
        Instantiate(weaponObject, gameObjectTransform);
        if (isPlayingAnim)
        {
            isBlocking = true;
            weaponAnimController.Play(animName);
            isPlayingAnim = false;
            yield break;
        }
        Destroy(weaponObject);
        isBlocking = false;
        yield return null; //I thought of doing an event but realized that you can do that and I don't care. Good luck!
    }

    public virtual IEnumerator playWeaponSound(AudioClip audioClipName)
    {
        weaponAudioSource.clip = audioClipName;
        weaponAudioSource.Play();
        return null;
    }
}
