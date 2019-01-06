using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio File", menuName = "ScriptableObjects/Custom Audio File", order = 1000)]
public class AudioScriptableObject : ScriptableObject {

    [Tooltip("This is the name that will be searched for when playing a sound")]
    public string audioName;
    public AudioClip clip;
    [Range(0, 1)]
    public float defaultVolume;
}