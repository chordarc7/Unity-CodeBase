using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    [CreateAssetMenu(fileName = "Sound", menuName = "Sound")]
    public class Sound : ScriptableObject
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        public float volume = 1f;
        public float pitch = 1f;

        public AudioMixerGroup group;
    }
}
