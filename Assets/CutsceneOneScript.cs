using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneOneScript : MonoBehaviour
{
    [SerializeField] DialogueScript dialogueScript;
    [SerializeField] PlayableDirector playableDirector;
    int currentTime;
    bool s1;
    bool s2;
    bool s3;
    bool s4;
    bool s5;
    bool s6;
    bool s7;
    bool s8;
    bool s9;
    bool s10;
    bool s11;
    bool s12;
    bool s13;
    bool s14;

    private void Start()
    {
        s1 = false;
        s2 = false;
        s3 = false;
        s4 = false;
        s5 = false;
        s6 = false;
        s7 = false;
        s8 = false;
        s9 = false;
        s10 = false;
        s11 = false;
        s12 = false;
        s13 = false;
        s14 = false;
       
    }
    void Update()
    {
        currentTime = (int)playableDirector.time;

        if(!s1 && currentTime == 9)
        {
            dialogueScript.dialogue("I'm so tired...",0.25f);
            s1 = true;
        }

        if (!s2 && currentTime == 20)
        {
            dialogueScript.dialogue("I wish I can just close my eyes and sleep...", 0.05f);
            s2 = true;
        }
        if (!s3 && currentTime == 26)
        {
            dialogueScript.dialogue("forever...", 0.3f);
            s3 = true;
        }
        if (!s4 && currentTime == 34)
        {
            dialogueScript.dialogue("and ever...", 0.35f);
            s4 = true;
        }
        if(!s5 && currentTime == 50)
        {
            dialogueScript.dialogue("No... I can't.", 0.2f);
            s5 = true;
        }
        if (!s6 && currentTime == 55)
        {
            dialogueScript.dialogue("I won't be able to see Lily.", 0.1f);
            s6 = true;
        }
        if( !s7 && currentTime == 63)
        {
            dialogueScript.dialogue("I wonder how she'll tie her hair this time.", 0.1f);
            s7 = true;
        }
        if ( !s8 && currentTime == 70)
        {
            dialogueScript.dialogue("Braided?", 0.01f);
            s8 = true;
        }
        if (!s9 && currentTime == 72)
        {
            dialogueScript.dialogue("Ponytail?", 0.01f);
            s9 = true;
        }
        if (!s10 && currentTime == 74)
        {
            dialogueScript.dialogue("Or maybe one of those French things she likes to do?", 0.01f);
            s10 = true;
        }
        if (!s11 && currentTime == 80)
        {
            dialogueScript.dialogue("She'll look beautiful anyways.", 0.05f);
            s11 = true;
        }
        if(!s12 && currentTime == 90)
        {
            dialogueScript.dialogue("She's always beautiful.", 0.1f);
            s12 = true;
        }
        if (!s13 && currentTime == 110)
        {
            dialogueScript.dialogue("I- I just-", 0.1f);
            s13 = true;
        }
        if (!s14 && currentTime == 116)
        {
            dialogueScript.dialogue("I hope she'll for-", 0.1f);
            s14 = true;
        }
    }
}
