using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class HierarchyHelp {

    public static List<Transform> FillListWithChildren(Transform parent)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        List<Transform> toReturn = new List<Transform>();

        for (int i = 1; i < allChildren.Length; i++)
        {
            toReturn.Add(allChildren[i]);
        }

        return toReturn;
    }

    public static void ChangeLayerOfParentAndChildren(GameObject parent, int layerIndex)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.layer = layerIndex;
        }
    }

    public static Transform FindChildByName(Transform parent, string childName)
    {
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i++)
        {
            if (allChildren[i].gameObject.name == childName)
            {
                return allChildren[i];
            }
        }

        Debug.LogError("No child with name " + childName + " was found");
        return null;
    }

    public static bool IsInArray(string toCheck, string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInArray(int toCheck, int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInArray(object toCheck, object[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == toCheck)
            {
                return true;
            }
        }

        return false;
    }

    public static List<object> ConvertToList(object[] array)
    {
        List<object> toReturn = new List<object>();

        for (int i = 0; i < array.Length; i++)
        {
            toReturn.Add(array[i]);
        }

        return toReturn;
    }


    public static List<T> FindAllObjectsOfType<T>()
    {
        List<T> toReturn = new List<T>();

        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] allRootObjects = currentScene.GetRootGameObjects();

        for(int i = 0; i < allRootObjects.Length; i++)
        {
            T[] foundTypes = allRootObjects[i].GetComponentsInChildren<T>(true);

            for (int t = 0; t < foundTypes.Length; t++)
            {
                toReturn.Add(foundTypes[t]);
            }
        }

        return toReturn;
    }

    /*
    /// <summary>
    /// DOES NOT WORK
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minDegrees"></param>
    /// <param name="maxDegrees"></param>
    /// <returns></returns>
    public static float EulerClamp(float value, float minDegrees, float maxDegrees)
    {
        float toReturn = value;

        if (minDegrees < 0)
        {
            minDegrees = -minDegrees;
        }

        maxDegrees = 360 - maxDegrees;

        //Problem: 2nd if statement is checked too late; value is already past the second if statement and will be clamped by the first. Causing it to snap to minDegrees.
        //Temporary fix: Subtract 10 degrees from maxDegrees and add 10 to minDegrees
        //Will not work when value changes quickly
        if (value > 1 && value < minDegrees)
        {
            toReturn = Mathf.Clamp(value, 1, minDegrees);
            Debug.Log("0 " + toReturn);

            return toReturn;
        }
        else if (value < 359 && value > maxDegrees)
        {
            toReturn = Mathf.Clamp(value, maxDegrees, 359);
            Debug.Log("360 " + toReturn);

            return toReturn;
        }
        else
        {
            toReturn = Mathf.Clamp(value, 0, ClosestTo(value, minDegrees, maxDegrees));
        }


        return value;
    }
    */

    public static float ClosestTo(float value, float first, float second)
    {
        float valueCheck = value;
        float firstCheck = first;
        float secondCheck = second;

        firstCheck = first - valueCheck;
        firstCheck = CheckNegativity(firstCheck);

        secondCheck = second - valueCheck;
        secondCheck = CheckNegativity(secondCheck);

        if (firstCheck < secondCheck)
        {
            return first;
        }
        else if (secondCheck < firstCheck)
        {
            return second;
        }

        return first;
    }

    public static float CheckNegativity(float value)
    {
        if (value < 0)
        {
            float toReturn = -value;
            return toReturn;
        }

        return value;
    }

    public static int CheckNegativity(int value)
    {
        if (value < 0)
        {
            int toReturn = -value;
            return toReturn;
        }

        return value;
    }

    public static float CreateAxis(float value, float min, float max)
    {
        float newMin = CheckNegativity(min);
        float newMax = 360 - max;

        if (value > 0 && value < newMin)
        {
            return value / -newMin;
        }
        else if (value < 360 && value > newMax)
        {
            return (360 - value) / (360 - newMax);
        }
        else if (value > newMin && value < 180)
        {
            return -1;
        }
        else if (value < newMax && value > 180)
        {
            return 1;
        }

        return 0;
    }

    public static int NextInArray(object[] array, int currentIndex, int toAdd)
    {
        if (currentIndex + toAdd > 0)
        {
            if (currentIndex + toAdd < array.Length)
            {
                return currentIndex + toAdd;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return array.Length - 1;
        }
    }
}
