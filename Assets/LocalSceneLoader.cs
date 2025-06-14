using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneLoader : MonoBehaviour
{
	public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
		if (SceneManager.GetActiveScene().buildIndex == 8)
		{
			gm = GameObject.FindObjectOfType<GameManager>();
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public void LoadScene(int scene)
	{
		SceneManager.LoadScene(scene);
	}
	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void loadNextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void loadEndScene() 
	{
		if(SceneManager.GetActiveScene().buildIndex == 8 && gm.notePiecesCollected >= 3)
		{
			SceneManager.LoadScene(9); // Load the main menu scene
		}
		else if(SceneManager.GetActiveScene().buildIndex == 8)
		{
			SceneManager.LoadScene(10); // Load next scene
		}
	}
}
