using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightAttackState : MonoBehaviour
{
    [SerializeField] public float radius;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private VisualDisorientScript visualDisorient;
    [SerializeField] private Spawner spawnerScript;

    private void Start()
    {
        visualDisorient = GetComponent<VisualDisorientScript>();
        visualDisorient.enabled = false;
        spawnerScript = GetComponent<Spawner>();
        spawnerScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInRadius())
        {
            visualDisorient.enabled = true;
            spawnerScript.enabled = true;
        }

        /*
        if ((GameObject.Find("VisualDisorient") == false) && canSpawnDisorient == false)
        {
            //(POSSIBLE FEATURE) play animation and do a final explode attack that charges for like 2 seconds to indicate to the player.
            //Afterwards, the enemy will be destroyed alongside that off all enemies in the explode radius.
            Destroy(gameObject);
        }
        */

    }

    public bool isInRadius() 
    {
        return Physics2D.OverlapCircle(transform.position, radius, playerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
