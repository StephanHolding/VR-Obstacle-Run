using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialWindow : UI_Window {

    public Transform[] tutorialPages;

    public int pageIndex = -1;

    private void Awake()
    {
        CheckActivePages();
    }

    public void CheckActivePages()
    {
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            if (tutorialPages[i].gameObject.activeInHierarchy)
            {
                pageIndex = i;
            }
        }
    }

    public void NextPage()
    {
        tutorialPages[pageIndex].gameObject.SetActive(false);
        pageIndex = HierarchyHelp.NextInArray(tutorialPages, pageIndex, 1);
        tutorialPages[pageIndex].gameObject.SetActive(true);
    }

    public void PreviousPage()
    {
        tutorialPages[pageIndex].gameObject.SetActive(false);
        pageIndex = HierarchyHelp.NextInArray(tutorialPages, pageIndex, -1);
        tutorialPages[pageIndex].gameObject.SetActive(true);
    }
}
