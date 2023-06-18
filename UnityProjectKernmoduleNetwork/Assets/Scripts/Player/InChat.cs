using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InChat : PlayerMovement
{
    private ChatBehaviour chatBehaviour;

    public override void Init()
    {
        base.Init();
        chatBehaviour = SessionVariables.instance.myGameClient.chatBehaviour;
    }

    public override void OnEnter()
    {
        inputHandler.pressOpenChatFirst += CloseChatWithMessageSend;
        inputHandler.pressEscapeFirst += CloseChatWithoutMessageSend;
        chatBehaviour.OpenChat();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void OnExit()
    {
        inputHandler.pressOpenChatFirst -= CloseChatWithMessageSend;
        inputHandler.pressEscapeFirst -= CloseChatWithoutMessageSend;
        chatBehaviour.CloseChat();
    }

    public override void OnUpdate()
    {
        GroundDetection();
        if (rb.useGravity) RotateTowardsGravity(Physics.gravity);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (isGrounded) ReduceSpeed(playerSheet.groundMaxSpeed, playerSheet.groundMoveSmoothTime, playerSheet.groundMoveSmoothTime);
        else ReduceSpeed(playerSheet.airMaxSpeed, playerSheet.airMoveSmoothTime, playerSheet.airNonMoveSmoothTime);
    }

    public override void OnLateUpdate() { }

    private void CloseChatWithMessageSend()
    {
        chatBehaviour.SendMessageToServer();
        if (isGrounded) stateManager.SwitchState(typeof(OnGround));
        else stateManager.SwitchState(typeof(InAir));
    }

    private void CloseChatWithoutMessageSend()
    {
        if (isGrounded) stateManager.SwitchState(typeof(OnGround));
        else stateManager.SwitchState(typeof(InAir));
    }
}
