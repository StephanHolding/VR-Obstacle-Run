using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameLetter : MonoBehaviour {

    public int letterIndex;
    public Text letter;

    private void OnEnable()
    {
        letter.text = HierarchyHelp.alphabet[letterIndex];
    }

    public void NextLetter()
    {
        letterIndex = HierarchyHelp.NextInArray(HierarchyHelp.alphabet, letterIndex, 1);
        letter.text = HierarchyHelp.alphabet[letterIndex];
    }

    public void PreviousLetter()
    {
        letterIndex = HierarchyHelp.NextInArray(HierarchyHelp.alphabet, letterIndex, -1);
        letter.text = HierarchyHelp.alphabet[letterIndex];
    }
}
