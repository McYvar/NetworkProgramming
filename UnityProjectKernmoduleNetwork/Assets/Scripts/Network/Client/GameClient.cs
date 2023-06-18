using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class GameClient : BaseClient
{
    public ChatBehaviour chatBehaviour;
    public PlayerSpawner playerSpawner;
    public PlayerTransformer playerTransformer;
    public PlayerRotator playerRotator;
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
                msg = new Net_StartGame(stream, deathRunGameLoop);
                break;
            case OpCode.OPEN_BARRIERS:
                msg = new Net_OpenBarriers(stream, deathRunGameLoop);
                break;
            case OpCode.CLOSE_BARRIERS:
                msg = new Net_CloseBarriers(stream, deathRunGameLoop);
                break;
            case OpCode.REACHED_GOAL:
                // only send by client, not received
                break;
            case OpCode.JOIN_GAME:
                // only send by client, not received
                break;
            case OpCode.LEAVE_GAME:
                // only send by client, not received
                break;
            case OpCode.PLAYER_GRAVITY:
                msg = new Net_PlayerGravity(stream, playerRotator);
                break;
            case OpCode.PLAYER_DIED:
                // only send by client, not received
                break;
            case OpCode.REACHED_CHECKPOINT:
                // only send by client, not received
                break;
            case OpCode.PLAYER_DISCONNECT:
                msg = new Net_Disconnect(stream, playerSpawner);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnClient();
    }
}