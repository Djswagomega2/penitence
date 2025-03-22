using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VisualDisorientScript : MonoBehaviour
{
    public CameraShake cameraShake;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float lengthOfDisorient;
    [SerializeField] private GameObject sightEnemy;


    private void Start()
    {
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        //sightEnemy = GameObject.Find("Sight Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if ((sightEnemy.GetComponent<SightAttackState>().isInRadius()) == false)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= lengthOfDisorient)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            destroyTimer = 0;
        }
        StartCoroutine(cameraShake.Shake(shakeMagnitude));
    }
}
