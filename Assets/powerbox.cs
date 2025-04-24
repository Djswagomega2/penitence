using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerbox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private BoxCollider2D pbCollider;
    [SerializeField] private GameManager gamemanager;
    void Start()
    {
        pbCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Powerbox hit");
            pbCollider.enabled = false;
            gamemanager.pbCounter++;
        }
    }
}
