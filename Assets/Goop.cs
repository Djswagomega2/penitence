using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goop : MonoBehaviour
{
    [SerializeField] GameObject puddle;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            Instantiate(puddle, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
