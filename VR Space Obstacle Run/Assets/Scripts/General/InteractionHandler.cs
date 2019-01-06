using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour {

    public enum InteractionMode 
    {
        Looking,
        UI,
        Flying,
    }

    public InteractionMode interactionMode;
}
