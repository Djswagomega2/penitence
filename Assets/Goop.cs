using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goop : MonoBehaviour
{
    [SerializeField] GameObject puddle;
    [SerializeField] Rigidbody2D goopRb;
    public SmellAttackState smellAttack;

	private void Start()
	{
		goopRb = GetComponent<Rigidbody2D>();
	}
	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            puddle.tag = "Puddle";
            Instantiate(puddle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Blocking")
		{
		    goopRb.velocity = -smellAttack.direction * smellAttack.goopSpeed;
		}
	}
}
