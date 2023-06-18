using UnityEngine;

public class InAir : PlayerMovement
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isGrounded) stateManager.SwitchState(typeof(OnGround));
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Movement(playerSheet.airForce);
        ReduceSpeed(isSprinting ? playerSheet.airMaxSprintSpeed : playerSheet.airMaxSpeed, playerSheet.airMoveSmoothTime, playerSheet.airNonMoveSmoothTime);
    }
}
