using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
	// Start is called before the first frame update
	public static GameManager instance;
	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private KeyCode pauseKey;
	[SerializeField] private bool isPaused;
	public int notePiecesCollected;
	[SerializeField] private bool allPiecesCollected;
	public int statuesInteracted;
	public GameObject door;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	private void Start()
	{
		pauseScreen = GameObject.FindGameObjectWithTag("PauseScreen");
		pauseScreen.SetActive(false);
		if (SceneManager.GetActiveScene().buildIndex.Equals(4)) 
		{
			door = GameObject.FindGameObjectWithTag("OpenedDoor");
		}
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
        }
		else
		{
			Time.timeScale = 1;
			pauseScreen.SetActive(false);
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

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
