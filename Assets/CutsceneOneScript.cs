using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneOneScript : MonoBehaviour
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
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    private int currentTime;
    private int nextLineIndex = 0;

    void Start()
    {
        // Optional: Sort the lines by timestamp at runtime
        dialogueLines.Sort((a, b) => a.timestamp.CompareTo(b.timestamp));
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
