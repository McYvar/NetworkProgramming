using UnityEngine;

public class Barriers : MonoBehaviour
{
    [SerializeField] private DeathRunGameLoop deathRunGameLoop;
    [SerializeField] private Vector3 displacement;
    [SerializeField] private float smoothTime;

    private Vector3 nextPos;
    private Vector3 velocity;

    private void Start()
    {
        deathRunGameLoop.allBarriers.Add(this);
        nextPos += transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, smoothTime);
    }

    public void OpenBarriers()
    {
        nextPos += displacement;
    }

    public void CloseBarriers()
    {
        nextPos -= displacement;
    }
}
