using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsCounter;
    // Start is called before the first frame update
    void Start()
    {
        fpsCounter.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter.text = "" + (int)(1f / Time.deltaTime);
    }
}
