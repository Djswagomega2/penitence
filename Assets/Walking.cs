using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    public Animator walkAnimator;
    public bool isWalking;
    public float animSpeed = 1.0f;
    public float sprintMultiplier = 2.0f;
    private bool key_up, key_down, key_left, key_right;
    // Start is called before the first frame update
    void Start()
    {
       walkAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        key_up = Input.GetKey(KeyCode.W);
        key_down = Input.GetKey(KeyCode.S);
        key_left = Input.GetKey(KeyCode.A);
        key_right = Input.GetKey(KeyCode.D);

        isWalking = (key_left || key_up || key_down || key_right);
        if (isWalking)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                walkAnimator.speed = animSpeed * sprintMultiplier;
            }
            else
            {
                walkAnimator.speed = animSpeed;
            }
        }
        else
        {
            walkAnimator.speed = 0;
        }
    }
}
