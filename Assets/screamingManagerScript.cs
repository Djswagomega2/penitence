using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screamingManagerScript : MonoBehaviour
{

    //if youre wondering why I have this here instead of my earlier stuff I lost the sound scripting so I was too lazy and just made a new script.
    //It is a little less performant, but honestly who cares.
    public GameObject playerBody;
    public GameObject soundEnemy;
    public float enemyDistanceScaler;
    public AudioSource enemyScreamSource; //specifically only handles screaming since you dont want it to be stopped.
    public AudioClip screamClip; 

    // Update is called once per frame
    void Start()
    {
        enemyScreamSource.clip = screamClip;
        enemyScreamSource.loop = true;
        enemyScreamSource.Play();
    }

    void Update()
    {
        enemyDistanceScaler = 1 / Vector2.Distance(playerBody.transform.position, this.transform.position);
        enemyScreamSource.volume = enemyDistanceScaler;
        Debug.Log(enemyDistanceScaler);
        Debug.Log(enemyScreamSource.volume);
    }
}
