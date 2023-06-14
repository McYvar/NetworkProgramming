using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameClient : BaseClient
{
    public static SessionVariables mySession = new SessionVariables();
    public Dictionary<int, GameObject> playerDictionary = new Dictionary<int, GameObject>(); // player id, player

    public static ChatBehaviour chatBehaviour;

    public override void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream, chatBehaviour);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnClient();
    }
}