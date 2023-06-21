using System.Collections.Generic;
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

    private List<IGravity> gravityObjects = new List<IGravity>();

    private Collider myCollider;

    private void Start()
    {
        if (gravityType != GravityType.STATIC_ZONE)
        {
            GetComponent<SphereCollider>().radius = zoneRadius;
        }

        myCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        
        foreach (var obj in gravityObjects)
        {
            if (gravityType == GravityType.STATIC_ZONE)
            {
                obj.SetGravity(gravityDirection.normalized * gravityStrenght);
            }
            if (gravityType == GravityType.GRAVITY_POINT_PULL)
            {
                obj.SetGravity((transform.position - obj.GetPosition()).normalized * gravityStrenght);
            }
            if (gravityType == GravityType.GRAVITY_POINT_PUSH)
            {
                obj.SetGravity((obj.GetPosition() - transform.position).normalized * gravityStrenght);
            }
        }
        
        for (int i = 0; i < gravityObjects.Count; i++)
        {
            if (!CheckBounds(gravityObjects[i].GetBounds()))
            {
                gravityObjects[i].OnExitZone();
                gravityObjects.RemoveAt(i);
                --i;
            }
        }
    }

    private bool CheckBounds(Bounds obj)
    {
        return myCollider.bounds.Intersects(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        IGravity obj = other.GetComponent<IGravity>();
        if (obj != null)
        {
            Vector3 resultDirection = Vector3.down;
            if (gravityType == GravityType.STATIC_ZONE)
            {
                resultDirection = gravityDirection.normalized * gravityStrenght;
            }
            if (gravityType == GravityType.GRAVITY_POINT_PULL)
            {
                resultDirection = (transform.position - obj.GetPosition()).normalized * gravityStrenght;
            }
            if (gravityType == GravityType.GRAVITY_POINT_PUSH)
            {
                resultDirection = (obj.GetPosition() - transform.position).normalized * gravityStrenght;
            }
            gravityObjects.Add(obj);

            InputHandler inputHandler = other.GetComponent<InputHandler>();
            if (inputHandler != null)
            {
                SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, resultDirection.x, resultDirection.y, resultDirection.z));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InputHandler inputHandler = other.GetComponent<InputHandler>();
        if (inputHandler != null)
        {
            SessionVariables.instance.myGameClient.SendToServer(new Net_PlayerGravity(SessionVariables.instance.myPlayerId, Physics.gravity.x, Physics.gravity.y, Physics.gravity.z));
        }
    }
}

public enum GravityType { STATIC_ZONE = 0, GRAVITY_POINT_PULL = 1, GRAVITY_POINT_PUSH = 2 }