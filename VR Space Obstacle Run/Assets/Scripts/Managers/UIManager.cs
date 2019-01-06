using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    [System.Serializable]
    public struct Window
    {
        public string windowName;
        public Transform window;
    }

    public Window[] windows;
    public Transform menuCanvas;

    private Transform activeWindow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowWindow(string windowName)
    {
        if (!menuCanvas.gameObject.activeInHierarchy)
        {
            menuCanvas.gameObject.SetActive(true);
        }


        Transform toShow = GetWindow(windowName);

        if (activeWindow != null)
        activeWindow.gameObject.SetActive(false);
        toShow.gameObject.SetActive(true);
        activeWindow = toShow;
    }

    public void CloseAllWindows()
    {
        activeWindow.gameObject.SetActive(false);

        if (menuCanvas.gameObject.activeInHierarchy)
            menuCanvas.gameObject.SetActive(false);
    }

    private Transform GetWindow(string windowName)
    {
        for (int w = 0; w < windows.Length; w++)
        {
            if (windowName == windows[w].windowName)
            {
                return windows[w].window;
            }
        }

        Debug.LogError("There is no window with the name " + windowName);
        return null;
    }
}
