using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class GameClient : BaseClient
{
    public ChatBehaviour chatBehaviour;
    public PlayerSpawner playerSpawner;
    public PlayerTransformer playerTransformer;
    public TrapsHandler trapsHandler;
    public PlayerTeleporter playerTeleporter;
    public DeathRunGameLoop deathRunGameLoop;

    public override void OnData(DataStreamReader stream)
    {
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
            case OpCode.ACTIVATE_TRAP:
                msg = new Net_ActivateTrap(stream, trapsHandler);
                break;
            case OpCode.TELEPORT_PLAYER:
                msg = new Net_TeleportPlayer(stream, playerTeleporter);
                break;
            case OpCode.START_GAME:
                // nothing happens here client sided
                break;
            case OpCode.START_ROUND:
                msg = new Net_StartRound(stream, deathRunGameLoop);
                break;
            case OpCode.END_ROUND:
                msg = new Net_EndRound(stream, deathRunGameLoop);
                break;
            case OpCode.END_GAME:
                // nothing happens here client sided
                break;
            case OpCode.OPEN_BARRIERS:
                msg = new Net_OpenBarriers(stream, deathRunGameLoop);
                break;
            case OpCode.CLOSE_BARRIERS:
                msg = new Net_CloseBarriers(stream, deathRunGameLoop);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnClient();
    }
}