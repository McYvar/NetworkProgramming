using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] int trapId;
    [SerializeField] private TrapsHandler trapsHandler;
    [SerializeField] private float trapResetTime;
    [SerializeField] private Vector3 displacement;
    [SerializeField, Range(0, 1)] private float smoothTime;
    private Vector3 targetPos;
    private Vector3 velocity;
    private float timer;
    private bool active = false;

    private void Start()
    {
        targetPos = transform.position;
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

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void ActivateTrap()
    {
        timer = trapResetTime;
        targetPos += displacement;
        active = true;
    }

    public void ResetTrap()
    {
        targetPos -= displacement;
    }
}
