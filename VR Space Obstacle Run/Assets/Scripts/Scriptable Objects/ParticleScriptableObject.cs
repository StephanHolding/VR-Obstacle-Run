using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Custom Particle", menuName = "ScriptableObjects/Custom Particle", order = 1000)]
public class ParticleScriptableObject : ScriptableObject {

    public string particleName;
    public ParticleSystem particle;
    public Vector3 defaultScaling;
}
