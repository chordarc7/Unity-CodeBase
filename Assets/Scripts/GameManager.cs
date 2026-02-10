using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;

    private ObjectResolver resolver;

    private void Awake()
    {
        InitResolver();
        TestSound();
    }

    private void InitResolver()
    {
        resolver = new ObjectResolver();
        resolver.RegisterInstance(Mixer);
        resolver.Register<SoundManager>();
    }

    private void TestSound()
    {
        SoundManager soundManager = resolver.Resolve<SoundManager>();

        soundManager.PlaySound("BGM");
    }
}