using Pathfinding;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellAttackState : State
{
    #region General
    [Header("General")]
    [SerializeField] private bool showGizmos;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private FOV fov;
    [SerializeField] private float pursuitSpeed;
    #endregion

    #region AStarGrid and Scripts
    [Header("AStarGrid and Scripts")]
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    [SerializeField] private AILerp aiLerp;
    #endregion

    #region States to Transition to
    [Header("States to Transition to")]
    [SerializeField] private State wanderState;
    #endregion

    #region AStarGrid and Scripts
    [Header("Shooting Goop")]
    [SerializeField] float goopTimer;
    [SerializeField] float goopRespawn;
    [SerializeField] GameObject goop;
    [SerializeField] float goopSpeed;
    [SerializeField] float goopSpawnDistance;

    #endregion

    private void Start()
    {
        fov = enemy.GetComponent<FOV>();
        aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
        aiLerp = enemy.GetComponent<AILerp>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTransform = enemy.GetComponent<Transform>();
    }
    public override State RunCurrentState()
    {
        aiLerp.speed = pursuitSpeed;
        aiDestinationSetter.target = playerTransform;
        if (!fov.canSeePlayer)
        {
            aiDestinationSetter.target = null;
            return wanderState;
        }
        return this;
    }
    void Update()
    {
        if (fov.canSeePlayer)
        {
            goopTimer += Time.deltaTime;
            if (goopTimer >= goopRespawn)
            {
                Vector2 direction = (playerTransform.position - enemyTransform.position).normalized;
                GameObject newGoop = Instantiate(goop, enemyTransform.position + (Vector3)(direction * goopSpawnDistance), Quaternion.identity);
                Rigidbody2D goopRb = newGoop.GetComponent<Rigidbody2D>();
                if (goopRb != null)
                {
                    goopRb.velocity = direction * goopSpeed;
                }
                goopTimer = 0;
            }
        }
    }
}
