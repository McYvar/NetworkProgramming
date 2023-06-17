using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_StartGame : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_StartGame()
    {
        code = OpCode.START_GAME;
    }

    public Net_StartGame(DataStreamReader reader)
    {
        code = OpCode.START_GAME;
        Deserialize(reader);
    }

    public Net_StartGame(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.START_GAME;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.StartGame();
    }

    public override void ReceivedOnClient()
    {
    }
}

public class Net_ReachedGoal : NetMessage
{
    public int playerId { get; set; }
    private DeathRunGameLoop deathRunGameLoop;
    public Net_ReachedGoal()
    {
        code = OpCode.REACHED_GOAL;
    }

    public Net_ReachedGoal(int playerId)
    {
        code = OpCode.REACHED_GOAL;
        this.playerId = playerId;
    }

    public Net_ReachedGoal(DataStreamReader reader)
    {
        code = OpCode.REACHED_GOAL;
        Deserialize(reader);
    }

    public Net_ReachedGoal(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.REACHED_GOAL;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.ReachedGoal(playerId);
    }
}

public class Net_OpenBarriers : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_OpenBarriers()
    {
        code = OpCode.OPEN_BARRIERS;
    }

    public Net_OpenBarriers(DataStreamReader reader)
    {
        code = OpCode.OPEN_BARRIERS;
        Deserialize(reader);
    }
    public Net_OpenBarriers(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.OPEN_BARRIERS;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.OpenBarriers();
    }
}


public class Net_CloseBarriers : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_CloseBarriers()
    {
        code = OpCode.CLOSE_BARRIERS;
    }

    public Net_CloseBarriers(DataStreamReader reader)
    {
        code = OpCode.CLOSE_BARRIERS;
        Deserialize(reader);
    }
    public Net_CloseBarriers(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.CLOSE_BARRIERS;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.CloseBarriers();
    }
}

public class Net_JoinGame : NetMessage
{
    public int playerId;
    private DeathRunGameLoop deathRunGameLoop;

    public Net_JoinGame()
    {
        code = OpCode.JOIN_GAME;
    }
    public Net_JoinGame(int playerId)
    {
        code = OpCode.JOIN_GAME;
        this.playerId = playerId;
    }

    public Net_JoinGame(DataStreamReader reader)
    {
        code = OpCode.JOIN_GAME;
        Deserialize(reader);
    }

    public Net_JoinGame(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.JOIN_GAME;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.JoinPlayer(playerId);
    }
}

public class Net_LeaveGame : NetMessage
{
    public int playerId;
    private DeathRunGameLoop deathRunGameLoop;

    public Net_LeaveGame()
    {
        code = OpCode.LEAVE_GAME;
    }

    public Net_LeaveGame(int playerId)
    {
        code = OpCode.LEAVE_GAME;
        this.playerId = playerId;
    }

    public Net_LeaveGame(DataStreamReader reader)
    {
        code = OpCode.LEAVE_GAME;
        Deserialize(reader);
    }

    public Net_LeaveGame(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.LEAVE_GAME;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.LeavePlayer(playerId);
    }
}