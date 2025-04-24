using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    [SerializeField] private GameObject trainRoof;
    [SerializeField] private bool isPlayerInTrain = false;
    // Start is called before the first frame update
    void Start()
    {
        trainRoof = Instantiate(trainRoof, gameObject.transform.position, Quaternion.identity);
        //isPlayerInTrain = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrain)
        {
            trainRoof.SetActive(false);
        }
        if (isPlayerInTrain == false)
        {
            trainRoof.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the entering object is the player
        {
            isPlayerInTrain = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the exiting object is the player
        {
            isPlayerInTrain = false;
        }
    }
}
