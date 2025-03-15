using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectScript : MonoBehaviour
{
    public WeapClass WeapClassScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().ReceiveDamage(WeapClassScript.weaponDamage);
        }
    }
    
}
