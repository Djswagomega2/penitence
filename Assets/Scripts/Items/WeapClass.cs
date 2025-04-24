using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "new Weapon Class", menuName = "Item/Weapon")]
public class WeapClass : ItemClass
{
    //I just want to make something clear. The weapon object is separate from the weapon itself.
    //The weapon is an absrtact concept which controls the values of the weapon object.
    //This way, if we really wanna fuck around, we can make the changes without having to tear up too much infrastructure. 
    //Also, I am aware of itemobject. I don't know what it use cases that was intended for, so this will have to do. If bloat just remove and Ill rewrite. 
    //Also, I've only set it up for melee since I presume we're just doing hitscan since projectiles aren't really necessary.

    // old weapon shit (Nam)
    [Header("Weapon")]
    public WeaponType weaponType;
    public ThrowType throwableType;
    public int weaponDamage;
    public int ammoCapacity;
    public float _fireRate;
    public float firingError;
    public float damageFalloffRange;
    public float damageFalloff;
    public float reloadSpeed;
    public float throwSpeed;
    public AudioClip attackSound;
    public AudioClip reloadSound;
    public AudioClip emptySound;

    //new weapon shit (Toto) \
    [Header("Shitter variables")]
    //public Animation weaponAnimController; //controls the weapon anim //CANNOT USE
    public Dictionary<string, AnimationClip> weaponAnimList; //dict of anims to play from. First value is anim name, second value is anim clip.
    public float weaponDurability; //might be vestigial, have to recheck with team to see if this doesn't work out.
    public Vector2 offsetVector; //used to offset insantiation position of weaponobject.
    public float maxRange; //for editing from unity
    public Ray hitscanRay;
    public float blockDecimal;
    public bool isBlocking;

    public enum WeaponType { gun, melee, throwable }//why tf do we have this

    public enum ThrowType { bottle, rock }

}