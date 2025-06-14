using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndingCutsceneScript : MonoBehaviour
{
	[SerializeField] private DialogueScript dialogueScript;
	[SerializeField] private PlayableDirector playableDirector;
	[SerializeField] private GameManager gm;

	[System.Serializable]
	public class DialogueLine
	{
		public int timestamp;         // Time in seconds
		[TextArea] public string text; // Dialogue text
		public float delay = 0.1f;     // Optional delay per character
	}

	[Tooltip("Add your dialogue lines here, with their timestamps and delays.")]
	private List<DialogueLine> dialogueLines = new List<DialogueLine>();
	public List<DialogueLine> goodEndingLines = new List<DialogueLine>();
	public List<DialogueLine> badEndingLines = new List<DialogueLine>();


	private int currentTime;
	private int nextLineIndex = 0;

	void Start()
	{
		gm = GameObject.FindObjectOfType<GameManager>();
		// Optional: Sort the lines by timestamp at runtime
		if (gm.notePiecesCollected >= 3)
		{
			dialogueLines = goodEndingLines;
			dialogueLines.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
		}
		else
		{
			dialogueLines = badEndingLines;
			dialogueLines.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
		}
	}

	void Update()
	{
		currentTime = (int)playableDirector.time;

		if (nextLineIndex < dialogueLines.Count && currentTime >= dialogueLines[nextLineIndex].timestamp)
		{
			var line = dialogueLines[nextLineIndex];
			dialogueScript.dialogue(line.text, line.delay);
			nextLineIndex++;
		}

		if (playableDirector.state == PlayState.Paused)
		{
			gm.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load next scene when cutscene ends
		}
	}
}
