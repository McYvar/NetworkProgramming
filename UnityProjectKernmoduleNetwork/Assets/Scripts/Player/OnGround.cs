public class OnGround : PlayerMovement
{
    public override void OnEnter()
    {
        inputHandler.pressJumpFirst += Jump;
    }

    public override void OnExit()
    {
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
