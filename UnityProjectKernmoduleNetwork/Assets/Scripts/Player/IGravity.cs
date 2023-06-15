using UnityEngine;

public interface IGravity
{
    void OnEnterZone();
    void SetGravity(Vector3 direction);
    void OnExitZone();
    Vector3 GetPosition();
}