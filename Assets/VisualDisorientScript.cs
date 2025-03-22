using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Video;

public class VisualDisorientScript : MonoBehaviour
{
    //public CameraShake cameraShake;
    //[SerializeField] private float shakeMagnitude;
    [SerializeField] private float destroyTimer;
    [SerializeField] private float lengthOfDisorient;
    [SerializeField] private GameObject sightEnemy;
    [SerializeField] private GameObject disorientEffect;
    [SerializeField] private Volume volume;
    private ChromaticAberration chromer;

    private void Start()
    {
        destroyTimer = 10f;
        disorientEffect.SetActive(true);
        volume = disorientEffect.GetComponent<Volume>();
        
       /* if (volume.profile.HasSettings<ChromaticAberration>())
        {
            ppv.profile.TryGetSettings(out chromer);
        }
        ppv.enabled = true;*/
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer -= Time.deltaTime;
        //chromer.intensity.value = Mathf.Max(0, chromer.intensity.value - destroyTimer * 1.2f);
        
        if (chromer != null)
        {
            chromer.intensity.value -= destroyTimer * 10f;
            Debug.Log("This is being refercened dumb jew bitch");
        }
        Debug.Log(chromer.intensity.value);

        if (chromer.intensity.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
