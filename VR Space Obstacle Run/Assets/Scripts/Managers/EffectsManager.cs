using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EffectsManager : MonoBehaviour {

    public static EffectsManager instance;

    // --- Audio Variables --- \\
    [Header("Audio Variables")]
    public AudioScriptableObject[] availableAudioClips;
    public List<AudioSource> activeAudioSources = new List<AudioSource>();
    public int defaultAudioSourceAmount;

    // --- Particle Variables --- \\
    [Header("Particle Variables")]
    public ParticleScriptableObject[] availableParticles;
    public List<ParticleSystem> activeParticleSystems = new List<ParticleSystem>();

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

        for (int a = 0; a < defaultAudioSourceAmount; a++)
        {
            CreateNewAudioSource();
        }
    }

    // ======================================== Public Audio Functions ======================================== \\

    public void PlayAudio(AudioScriptableObject clip, string key, float pitch = 1, float spatialBlend = 0, Vector3 audioPosition = default(Vector3))
    {
        AudioSource source = FindAvailableAudioSource();
        source.clip = clip.clip;
        source.volume = clip.defaultVolume;
        source.pitch = pitch;
        source.spatialBlend = spatialBlend;
        source.transform.position = audioPosition;

        if (source.gameObject.GetComponent<Key>() != null)
            source.gameObject.GetComponent<Key>().key = key;
        else
            source.gameObject.AddComponent<Key>().key = key;

        source.Play();
    }

    public AudioScriptableObject FindAudioClip(string name)
    {
        for (int i = 0; i < availableAudioClips.Length; i++)
        {
            if (availableAudioClips[i].audioName == name)
            {
                return availableAudioClips[i];
            }
        }

        Debug.LogError("There is no Custom Audio Clip named " + name);
        return null;
    }

    public void AdjustPitch(string audioSourceKey, float pitch)
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
           if (activeAudioSources[i].GetComponent<Key>().key == audioSourceKey)
           {
                activeAudioSources[i].pitch = pitch;
           }
        }
    }

    // ======================================== Private Audio Functions ======================================== \\

    private AudioSource FindAvailableAudioSource()
    {
        for (int i = 0; i < activeAudioSources.Count; i++)
        {
            if (!activeAudioSources[i].isPlaying)
            {
                return activeAudioSources[i];
            }
        }

        return CreateNewAudioSource();
    }

    private AudioSource CreateNewAudioSource()
    {
        GameObject newObject = new GameObject
        {
            name = "Audio Source"
        };
        newObject.transform.SetParent(transform);
        AudioSource source = newObject.AddComponent<AudioSource>();
        activeAudioSources.Add(source);
        return source;
    }

    // ======================================== Public Particle Functions ======================================== \\

    public void PlayParticle(ParticleScriptableObject particle, string key, Vector3 particlePosition, Quaternion forwardDirection, bool loop = false)
    {
        ParticleSystem system = CreateNewParticleSystem(particle);
        system.gameObject.AddComponent<Key>().key = key;
        system.transform.position = particlePosition;
        system.transform.rotation = forwardDirection;

        var main = system.main;
        main.loop = loop;

        system.Play();

        StartCoroutine(ParticleStopTimer(system, system.main.duration));
    }

    public ParticleScriptableObject FindParticle(string name)
    {
        for (int i = 0; i < availableParticles.Length; i++)
        {
            if (availableParticles[i].particleName == name)
            {
                return availableParticles[i];
            }
        }

        Debug.LogError("There is no Custom Particle named " + name);
        return null;
    }

    // ======================================== Private Particle Functions ======================================== \\

    private ParticleSystem CreateNewParticleSystem(ParticleScriptableObject particle)
    {
        GameObject newObject = Instantiate(particle.particle.gameObject, transform.position, Quaternion.identity);
        

        return newObject.GetComponent<ParticleSystem>();
    }

    private IEnumerator ParticleStopTimer(ParticleSystem system, float timerAmount)
    {
        yield return new WaitForSeconds(timerAmount);
        system.Stop();
    }
}
