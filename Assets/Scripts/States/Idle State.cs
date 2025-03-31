using Pathfinding;
using UnityEngine;

public class IdleState : State
{

    #region General
    [Header("General")]
    public bool showGizmos;
    [SerializeField] private Animator animator;
	#endregion

	#region States to transition to
	[Header("States to transition to")]
    public State pursuitState;
    #endregion

    #region Tracking the Player
    [Header("Tracking the Player")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private FOV fov;
	#endregion


	public void Start()
	{
		fov = enemy.GetComponent<FOV>();
		animator = enemy.GetComponent<Animator>();
        animator.enabled = false;
	}

	public override State RunCurrentState()
    {
        if (fov.canSeePlayer)
        {
            showGizmos = false;
            animator.enabled = true;
			return pursuitState;
        }
        else 
        {
           showGizmos = true;
           animator.enabled = false;
		}

        return this;
    }


}