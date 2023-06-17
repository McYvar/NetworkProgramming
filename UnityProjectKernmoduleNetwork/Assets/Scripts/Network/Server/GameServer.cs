using Unity.Networking.Transport;
using UnityEngine;

public class GameServer : BaseServer
{
    public DeathRunGameLoop deathRunGameLoop;
    public override void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream);
                break;
            case OpCode.SPAWN_PLAYER:
                msg = new Net_SpawnPlayer(stream);
                break;
            case OpCode.PLAYER_TRANSFORM:
                msg = new Net_PlayerTransform(stream);
                break;
            case OpCode.ACTIVATE_TRAP:
                msg = new Net_ActivateTrap(stream);
                break;
            case OpCode.TELEPORT_PLAYER:
                msg = new Net_TeleportPlayer(stream);
                break;
            case OpCode.START_GAME:
                msg = new Net_StartGame(stream, deathRunGameLoop);
                break;
            case OpCode.START_ROUND:
                // only send by server, not received
                break;
            case OpCode.END_ROUND:
                // only send by server, not received
                break;
            case OpCode.END_GAME:
                msg = new Net_EndGame(stream, deathRunGameLoop);
                break;
            case OpCode.OPEN_BARRIERS:
                // only send by server, not received
                break;
            case OpCode.CLOSE_BARRIERS:
                // only send by server, not received
                break;
            case OpCode.REACHED_GOAL:
                msg = new Net_ReachedGoal(stream, deathRunGameLoop);
                break;
            case OpCode.JOIN_GAME:
                msg = new Net_JoinGame(stream, deathRunGameLoop);
                break;
            case OpCode.LEAVE_GAME:
                msg = new Net_LeaveGame(stream, deathRunGameLoop);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnServer(this);
    }
}
