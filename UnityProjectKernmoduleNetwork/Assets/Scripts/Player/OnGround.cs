using UnityEngine;

public class OnGround : PlayerMovement
{
    public override void OnEnter()
    {
        base.OnEnter();
        inputHandler.pressJumpFirst += Jump;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        inputHandler.pressJumpFirst -= Jump;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!isGrounded) stateManager.SwitchState(typeof(InAir));
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        Movement(playerSheet.groundForce);
        ReduceToMaxSpeed(playerSheet.groundMaxSpeed, playerSheet.groundSmoothTime);
    }
}
