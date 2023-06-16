using UnityEngine;

public class InAir : PlayerMovement
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isGrounded) stateManager.SwitchState(typeof(OnGround));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Movement(playerSheet.airForce);
        ReduceToMaxSpeed(playerSheet.airMaxSpeed, playerSheet.airSmoothTime);
    }
}
