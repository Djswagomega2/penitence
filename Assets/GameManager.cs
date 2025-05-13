using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
	[SerializeField] private bool isPaused;
	public int notePiecesCollected;
	[SerializeField] private bool allPiecesCollected;
	public int statuesInteracted;
	public GameObject door;


	private void Start()
	{
		pauseScreen.SetActive(false);
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
			pauseScreen.SetActive(true);
            player.GetComponent<PlayerScript>().enabled = false;
        }
		else
		{
			Time.timeScale = 1;
			pauseScreen.enabled = false;
		}

		if (statuesInteracted >= 7)
		{
			Destroy(door);
		}

	}

	public void LoadScene(int scene) 
	{ 
		SceneManager.LoadScene(scene);
    }

	//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); <-- Reloads the scene 
}
