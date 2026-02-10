using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    private readonly AudioMixer mixer;
    private AudioSource[] audioSources;
    private Sound[] sounds;
    private const string SoundsPath = "Sounds/";

    public enum VolumeType
    {
        Master,
        Music
    }

    public SoundManager(AudioMixer mixer)
    {
        this.mixer = mixer;

        Initialize();
    }

    private void Initialize()
    {
        sounds = Resources.LoadAll<Sound>(SoundsPath);
        Debug.Log($"found {sounds.Length} sounds");
        audioSources = new AudioSource[sounds.Length];

        GameObject audioSourceObject = new("AudioSources");
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = audioSourceObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }
    }

    public void SetVolume(VolumeType type, float volume)
    {
        mixer.SetFloat(Enum.GetName(typeof(VolumeType), type) + "Volume", GetdB(volume));
    }

    private static float GetdB(float value)
    {
        return Mathf.Log10(Mathf.Max(value, float.Epsilon)) * 20f;
    }

    public void PlaySound(string name)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying) continue;

            Sound sound = GetSound(name);
            source.clip = sound.Clip;
            source.loop = sound.Loop;
            source.volume = sound.Volume;
            source.pitch = sound.Pitch;
            source.outputAudioMixerGroup = sound.Group;
            source.Play();
            return;
        }
    }

    private Sound GetSound(string name)
    {
        return sounds.FirstOrDefault(sound => sound.Name == name);
    }
}
