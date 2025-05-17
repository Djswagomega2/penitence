using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemies;
    public bool playerSteppedThrough;
    void Start()
    {
        enemies.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemies.SetActive(true);
            playerSteppedThrough = true;
            Debug.Log("Yes");
        }
    }
}
