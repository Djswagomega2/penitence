using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundAttackStateScript : State
{
    #region General
    [Header("General")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject player;
    [SerializeField] private bool hasSoundAttacked;
    [SerializeField] private float coolDownSeconds;
    [SerializeField] private float coolLungeSecond;
    [SerializeField] private float soundAttackSpeed;
    [SerializeField] private PlayerScript ps;
    #endregion

    #region AStarGrid and Scripts
    [Header("AStarGrid and Scripts")]
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    [SerializeField] private AIPath aiPath;
    [SerializeField] private AILerp aiLerp;
    #endregion

    #region States to Transition to
    [Header("States to Transition to")]
    [SerializeField] private State pursuitState;
    #endregion

    #region random slop go
    public Animator enemyAnimator;
    public AnimationClip enemyAttackAnimationClip;
    public bool hasAttacked;
    public CircleCollider2D detectionCollider;
    public CircleCollider2D killCollider;
    public 
    #endregion


    void Start()
    {
        enemyAnimator = gameObject.GetComponent<Animator>();
        aiDestinationSetter = enemy.GetComponent<AIDestinationSetter>();
        aiPath = enemy.GetComponent<AIPath>();
        aiLerp = enemy.GetComponent<AILerp>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        ps = player.GetComponent<PlayerScript>();
    }
    public override State RunCurrentState()
    {
        aiLerp.speed = soundAttackSpeed;
        if (!hasSoundAttacked)
        {
            aiDestinationSetter.enabled = false;
            aiPath.enabled = false;
            StartCoroutine(TouchAttack());
        }
        else
        {
            StopCoroutine(TouchAttack());
            aiDestinationSetter.enabled = true;
            aiDestinationSetter.target = null;
            aiPath.enabled = true;
            hasSoundAttacked = false;
            return pursuitState;
        }

        return this;
    }

    private IEnumerator TouchAttack()
    {
        Lunge();
        enemyAnimator.SetTrigger("attackNow");
        detectionCollider.enabled = !detectionCollider.enabled;
        killCollider.enabled = killCollider.enabled;
        yield return new WaitForSeconds(coolDownSeconds);
        hasSoundAttacked = true;
    }


    private void Lunge()
    {
        Vector2 directionToPlayer = ((Vector2)ps.droplet.transform.position - rb.position).normalized;
        rb.velocity = directionToPlayer * aiLerp.speed; ;
    }
}

