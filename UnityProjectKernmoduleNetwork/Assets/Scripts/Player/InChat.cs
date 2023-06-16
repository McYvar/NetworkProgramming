using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InChat : PlayerMovement
{
    [SerializeField] private ChatBehaviour chatBehaviour;

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


    private void CloseChatWithMessageSend()
    {
        chatBehaviour.SendMessageToServer();
        stateManager.SwitchState(typeof(OnGround));
    }

    private void CloseChatWithoutMessageSend()
    {
        stateManager.SwitchState(typeof(OnGround));
    }
}
