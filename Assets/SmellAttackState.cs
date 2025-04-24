using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellAttackState : State
{
    //Direct hit 1/10th player's health
    //Puddle 1 tick of damage every second
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

    #region Shooting Goop
    [Header("Shooting Goop")]
    [SerializeField] private float goopTimer;
    [SerializeField] private float goopRespawn;
    [SerializeField] private GameObject goop;
    public float goopSpeed;
    [SerializeField] private float goopSpawnDistance;
    public Vector2 direction;
	#endregion

	#region Retreating
   [Header("Retreating")]
    private Rigidbody2D playerRb;
    private Transform retreatTarget;
    [SerializeField] private float distanceToRetreat;
    [SerializeField] private float retreatBuffer = 0.5f;
    [SerializeField] private float retreatSpeed;
    private bool isRetreating = false;

    [SerializeField] private float stateCommitTime = 1.0f; // Prevents rapid switching
    private float stateTimer = 0f;
    #endregion

    private void Start()
    {
        fov = enemy.GetComponent<FOV>();
        aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
        aiLerp = enemy.GetComponent<AILerp>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTransform = enemy.GetComponent<Transform>();
        playerRb = playerTransform.GetComponent<Rigidbody2D>();
        retreatTarget = new GameObject("RetreatTarget").transform;
    }

    public override State RunCurrentState()
    {
        aiDestinationSetter.target = playerTransform;
        if (!fov.canSeePlayer)
        {
            aiDestinationSetter.target = null;
            return wanderState;
        }
        return this;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(playerTransform.position, enemyTransform.position);
        Vector2 directionToPlayer = (playerTransform.position - enemyTransform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        enemyTransform.rotation = Quaternion.Euler(0, 0, angle);

        stateTimer -= Time.deltaTime; // Decrease commit timer

        if (isRetreating)
        {
            if (distanceToPlayer > distanceToRetreat + retreatBuffer && stateTimer <= 0)
            {
                isRetreating = false;
                stateTimer = stateCommitTime; // Reset commit timer
            }
        }
        else
        {
            if (distanceToPlayer <= distanceToRetreat && stateTimer <= 0)
            {
                isRetreating = true;
                stateTimer = stateCommitTime; // Reset commit timer
            }
        }

        if (isRetreating)
        {
            Debug.Log("Retreating!");
            Vector2 retreatDirection = -directionToPlayer;
            retreatTarget.position = (Vector2)enemyTransform.position + retreatDirection * 3;
            aiDestinationSetter.target = retreatTarget;
            aiLerp.speed = retreatSpeed;
        }
        else
        {
            Debug.Log("Chasing Player!");
            aiDestinationSetter.target = playerTransform;
            aiLerp.speed = pursuitSpeed;
        }

        shootAttack();
    }

    public void shootAttack()
    {
        if (fov.canSeePlayer)
        {
            goopTimer += Time.deltaTime;
            if (goopTimer >= goopRespawn)
            {
                direction = (playerTransform.position - enemyTransform.position).normalized;
                GameObject newGoop = Instantiate(goop, enemyTransform.position + (Vector3)(direction * goopSpawnDistance), Quaternion.identity);
                Rigidbody2D goopRb = newGoop.GetComponent<Rigidbody2D>();
                Goop goopScript = newGoop.GetComponent<Goop>();
                goopScript.smellAttack = this;
				if (goopRb != null)
                {
                    goopRb.velocity = direction * goopSpeed;
                }
                goopTimer = 0;
            }
        }
    }
}
