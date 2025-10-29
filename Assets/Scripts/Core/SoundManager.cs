using Core;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager
{
    private readonly AudioMixer _mixer;
    private AudioSource[] _audioSources;
    private Sound[] _sounds;
    private const string SoundsPath = "Sounds/";
    
    public enum VolumeType
    {
        Master,
        Music
    }
    
    public SoundManager(AudioMixer mixer)
    {
        _mixer = mixer;
        
        Initialize();
    }

    private void Initialize()
    {
        _sounds = Resources.LoadAll<Sound>(SoundsPath);
        Debug.Log($"found {_sounds.Length} sounds");
        _audioSources = new AudioSource[_sounds.Length];
        
        GameObject audioSourceObject = new("AudioSources");
        for (int i = 0; i < _audioSources.Length; i++)
        {
            _audioSources[i] = audioSourceObject.AddComponent<AudioSource>();
            _audioSources[i].playOnAwake = false;
        }
    }
    
    public void SetVolume(VolumeType type, float volume)
    {
        _mixer.SetFloat(Enum.GetName(typeof(VolumeType), type) + "Volume", GetdB(volume));
    }

    private static float GetdB(float value)
    {
        return Mathf.Log10(Mathf.Max(value, float.Epsilon)) * 20f;
    }

    public void PlaySound(string name)
    {
        foreach (AudioSource source in _audioSources)
        {
            if (source.isPlaying) continue;
            
            Sound sound = GetSound(name);
            source.clip = sound.clip;
            source.loop = sound.loop;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.outputAudioMixerGroup = sound.group;
            source.Play();
            return;
        }
    }

    private Sound GetSound(string name)
    {
        return _sounds.FirstOrDefault(sound => sound.name == name);
    }
}
