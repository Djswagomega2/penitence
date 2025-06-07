using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMove : MonoBehaviour
{
    public HingeJoint2D hingeJoint;
    public Rigidbody2D rb;
    public GameObject pushPoint;
    public float pushForceMultiplier = 10f;

    [SerializeField] private GameObject player;
    private Rigidbody2D playerRb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cache player reference and Rigidbody
            if (player == null)
            {
                player = collision.gameObject;
                playerRb = player.GetComponent<Rigidbody2D>();
            }

            Vector3 playerVelocity = playerRb.velocity;

            // Check if player is pushing toward the door
            Vector3 doorToPlayer = (transform.position - player.transform.position).normalized;
            float pushDot = Vector3.Dot(playerVelocity.normalized, doorToPlayer);

            if (pushDot > 0.5f) // pushing into the door
            {
                // Apply force to the door at the push point
                rb.AddForceAtPosition(playerVelocity * pushForceMultiplier, pushPoint.transform.position);
            }
        }
    }
}

