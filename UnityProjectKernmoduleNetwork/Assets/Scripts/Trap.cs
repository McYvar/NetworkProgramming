using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] int trapId;
    [SerializeField] private TrapsHandler trapsHandler;
    [SerializeField] private float trapResetTime;
    [SerializeField] private Vector3 displacement;
    private float timer;
    private bool active = false;

    private void Start()
    {
        try
        {
            trapsHandler.AddToTraps(trapId, this);
        }
        catch
        {
            Debug.LogWarning($"Trap with id {trapId} already exists!");
        }
    }

    private void Update()
    {
        if (active)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                ResetTrap();
                active = false;
            }

        }
    }

    public void ActivateTrap()
    {
        timer = trapResetTime;
        transform.position += displacement;
        active = true;
    }

    public void ResetTrap()
    {
        transform.position -= displacement;
    }
}
