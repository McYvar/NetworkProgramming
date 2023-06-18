using UnityEngine;

public class GravityBehaviour : MonoBehaviour
{
    /// <summary>
    /// Date: 08/04/23, By: Yvar
    /// The idea is to create a zone where the player has a static gravity direction or a dynamic one.
    /// For example, if the zone is setup to have a static gravity direction, the players downward gravity
    /// is always towards this direction. If it is a dynamic zone, then it will be more of a circular zone.
    /// The pivot of the zone can be a pushing or pulling gravity. The strenght of the gravity can also be set.
    /// While I only say player here, it should be more of a interface that should be targeted.
    /// </summary>

    [SerializeField] GravityType gravityType = GravityType.STATIC_ZONE;

    [Space(10), Header("gravityDirection will be normalized"), SerializeField] Vector3 gravityDirection = Vector3.down;
    [SerializeField] float gravityStrenght = 9.81f;

    [Space(10), Header("zone radius only applied when gravity point"), SerializeField] float zoneRadius = 10;

    private void Start()
    {
        if (gravityType != GravityType.STATIC_ZONE)
        {
            GetComponent<SphereCollider>().radius = zoneRadius;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            IGravity obj = other.GetComponent<IGravity>();
            if (obj != null)
            {
                if (gravityType == GravityType.STATIC_ZONE)
                {
                    Vector3 resultDirection = gravityDirection.normalized * gravityStrenght;
                    obj.SetGravity(resultDirection);
                    SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, resultDirection.x, resultDirection.y, resultDirection.z));
                }
                if (gravityType == GravityType.GRAVITY_POINT_PULL)
                {
                    Vector3 resultDirection = (transform.position - obj.GetPosition()).normalized * gravityStrenght;
                    obj.SetGravity(resultDirection);
                    SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, resultDirection.x, resultDirection.y, resultDirection.z));
                }
                if (gravityType == GravityType.GRAVITY_POINT_PUSH)
                {
                    Vector3 resultDirection = (obj.GetPosition() - transform.position).normalized * gravityStrenght;
                    obj.SetGravity(resultDirection);
                    SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, resultDirection.x, resultDirection.y, resultDirection.z));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            IGravity obj = other.GetComponent<IGravity>();
            if (obj != null)
            {
                obj.OnExitZone();
                SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, Physics.gravity.x, Physics.gravity.y, Physics.gravity.z));
            }
        }
    }
}

public enum GravityType { STATIC_ZONE = 0, GRAVITY_POINT_PULL = 1, GRAVITY_POINT_PUSH = 2 }