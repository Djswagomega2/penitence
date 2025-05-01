using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Canvas pauseScreen;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
	[SerializeField] private bool isPaused;
	public int notePiecesCollected;
	[SerializeField] private bool allPiecesCollected;
	public int statuesInteracted;
	public bool allStatuesInteracted;


    private void Start()
	{
		pauseScreen.enabled = false;
	}
	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(pauseKey))
		{
			isPaused = !isPaused;
		}

		if (isPaused)
		{
			Time.timeScale = 0;
			pauseScreen.enabled = true;
		}
		else
		{
			Time.timeScale = 1;
			pauseScreen.enabled = false;
		}
	}

}
