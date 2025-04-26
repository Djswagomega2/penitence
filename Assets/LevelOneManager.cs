using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : MonoBehaviour
{
    public int generatorAmountTurnedOn;
    public GameObject closedTurnstile;
    public GameObject openTurnstile;
    // Start is called before the first frame update
    void Start()
    {
        generatorAmountTurnedOn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (generatorAmountTurnedOn >= 3)
        {
            
            openTurnstile = Instantiate(openTurnstile, closedTurnstile.transform.position, Quaternion.identity);
            Destroy(closedTurnstile);
            Debug.Log("Beat level");
        }
    }
}
