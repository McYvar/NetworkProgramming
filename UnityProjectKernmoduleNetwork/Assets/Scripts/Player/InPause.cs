using UnityEngine;

public class InPause : PlayerMovement
{
    public override void OnEnter()
    {
        PauseMenu.instance.gameObject.SetActive(true);
        PauseMenu.instance.SubscribeToButton(ClosePauseMenu);
        inputHandler.pressEscapeFirst += ClosePauseMenu;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void OnExit()
    {
        PauseMenu.instance.UnsubscribeFromButton(ClosePauseMenu);
        PauseMenu.instance.gameObject.SetActive(false);
        inputHandler.pressEscapeFirst -= ClosePauseMenu;
    }
    public override void OnUpdate()
    {
        GroundDetection();
        if (rb.useGravity) FallTowardsGravity(Physics.gravity);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (isGrounded) ReduceSpeed(playerSheet.groundMaxSpeed, playerSheet.groundMoveSmoothTime, playerSheet.groundMoveSmoothTime);
        else ReduceSpeed(playerSheet.airMaxSpeed, playerSheet.airMoveSmoothTime, playerSheet.airNonMoveSmoothTime);
    }

    public override void OnLateUpdate() => CameraMovement(0);

    private void ClosePauseMenu()
    {
        if (isGrounded) stateManager.SwitchState(typeof(OnGround));
        else stateManager.SwitchState(typeof(InAir));
    }
}