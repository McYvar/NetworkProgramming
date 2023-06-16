using UnityEngine;

public class Barriers : MonoBehaviour
{
    [SerializeField] private Vector3 displacement;

    public void OpenBarriers()
    {
        transform.position += displacement;
    }

    public void CloseBarriers()
    {
        transform.position -= displacement;
    }
}
