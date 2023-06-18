using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    private static GlobalSettings instance;
    public static float sensitivity = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
}