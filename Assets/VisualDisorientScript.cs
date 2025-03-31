using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VisualDisorientScript : MonoBehaviour
{
    [SerializeField] private float destroyTimer = 5f;
    [SerializeField] private GameObject disorientEffect;
    [SerializeField] private GameObject panelFlash;
    [SerializeField] private Image flash;
    private Volume postProcessingVolume;

    void Start()
    {
        disorientEffect.SetActive(true);
        panelFlash.SetActive(true);
        postProcessingVolume = disorientEffect.GetComponent<Volume>();
    }

    void Update()
    {
        destroyTimer -= Time.deltaTime;
        Color currentColor = flash.color;
        currentColor.a -= Time.deltaTime / destroyTimer;
        currentColor.a = Mathf.Max(currentColor.a, 0);
        flash.color = currentColor;
        if (postProcessingVolume != null)
        {
            if (postProcessingVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
            {
                chromaticAberration.intensity.Override(destroyTimer);
            }
            if (postProcessingVolume.profile.TryGet(out LensDistortion lensDistortion))
            {
                lensDistortion.intensity.Override(destroyTimer);
            }
        }

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}