using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    float growthTimer;
    float shrinkTimer;
    float delayTimer;
    [SerializeField] float growthLimit;
    [SerializeField] float shrinkLimit;
    [SerializeField] float delayLimit;
    [SerializeField] bool canGrow;
    [SerializeField] Vector3 growthRate;
    [SerializeField] Vector3 decayRate;
    private void Start()
    {
        canGrow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGrow)
        {
            //transform.localScale += Vector3.one * Time.deltaTime;
            transform.localScale += growthRate * Time.deltaTime;
            growthTimer += Time.deltaTime;
            if (growthTimer >= growthLimit)
            {
                canGrow = false;
                shrinkTimer = 0f;
            }
        }
        else
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayLimit)
            {
                //transform.localScale -= Vector3.one * Time.deltaTime;
                transform.localScale += decayRate * Time.deltaTime;
                shrinkTimer += Time.deltaTime;
                if (shrinkTimer >= shrinkLimit)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
