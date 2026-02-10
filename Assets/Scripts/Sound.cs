using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;
    public bool Loop;
    public float Volume = 1f;
    public float Pitch = 1f;

    public AudioMixerGroup Group;
}