using Core;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        
        private ObjectResolver _resolver;

        private void Awake()
        {
            InitResolver();
            TestSound();
        }

        private void InitResolver()
        {
            _resolver = new ObjectResolver();
            _resolver.RegisterInstance(mixer);
            _resolver.Register<SoundManager>();
        }

        private void TestSound()
        {
            SoundManager soundManager = _resolver.Resolve<SoundManager>();
            
            soundManager.PlaySound("BGM");
        }
    }
}
