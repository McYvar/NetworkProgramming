using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    private static GlobalSettings instance;
    public static float sensitivity = 1;
    public static float startingSensitivity;
    [SerializeField] private float sensitivityScalar;

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
    private void Start()
    {
        sensitivity *= sensitivityScalar;
        startingSensitivity = sensitivity;
    }

    public static void SetSensitivity(float newSens)
    {
        sensitivity = startingSensitivity * newSens;
    }
}