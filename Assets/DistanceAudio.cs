using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAudio: MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject player;
    [SerializeField] private float maxHearingDistance = 10f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = GetDistance(player, gameObject);

        // Clamp volume between 0 and 1 based on distance
        float volume = Mathf.Clamp01(1 - (distance / maxHearingDistance));
        audioSource.volume = volume;
        //Debug.Log("Distance: " + distance + " | Volume: " + volume);
    }

    float GetDistance(GameObject obj1, GameObject obj2)
    {
        return Vector2.Distance(obj1.transform.position, obj2.transform.position);
    }
}
