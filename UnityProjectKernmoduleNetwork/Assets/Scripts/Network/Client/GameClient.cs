using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameClient : BaseClient
{
    public ChatBehaviour chatBehaviour;
    public PlayerSpawner playerSpawner;
    public PlayerTransformer playerTransformer;

    public override void OnData(DataStreamReader stream)
    {
        Debug.Log("Received package on client!");
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream, chatBehaviour);
                break;
            case OpCode.SPAWN_PLAYER:
                msg = new Net_SpawnPlayer(stream, playerSpawner);
                break;
            case OpCode.PLAYER_TRANSFORM:
                msg = new Net_PlayerTransform(stream, playerTransformer);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnClient();
    }
}