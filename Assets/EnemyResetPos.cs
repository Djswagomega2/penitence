using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyResetPos : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Transform[] childTransforms;
    [SerializeField] private Vector2[] originalPos;


	// Start is called before the first frame update
	void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        for (int i = 0; i < childTransforms.Length; i++) 
        {
            childTransforms[i].position = originalPos[i];
        }
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartPosition() 
    {
        /*if (playerScript.health <= 0) 
        {
            
        }*/
    }
}
