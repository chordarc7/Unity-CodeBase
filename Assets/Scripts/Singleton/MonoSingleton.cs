using UnityEngine;

/// <summary>
/// Generic Singleton that inherits MonoBehaviour
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static bool ShuttingDown;
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (ShuttingDown) return null;

            // Find from world if null
            if (instance == null) instance = (T)FindFirstObjectByType(typeof(T));

            // Create instance if instance doesn't exist
            if (instance == null)
            {
                GameObject temp = new (typeof(T) + "_Instance");
                instance = temp.AddComponent<T>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    public void Init() { }

    private void OnDestroy()
    {
        if (instance == this)
        {
            OnCleanUp();
            instance = null;
        }
    }

    protected virtual void OnCleanUp() { }

    private void OnApplicationQuit()
    {
        ShuttingDown = true;
    }
}
