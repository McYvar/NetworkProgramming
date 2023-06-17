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


public class Net_StartRound : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_StartRound()
    {
        code = OpCode.START_ROUND;
    }

    public Net_StartRound(DataStreamReader reader)
    {
        code = OpCode.START_ROUND;
        Deserialize(reader);
    }

    public Net_StartRound(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.START_ROUND;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.StartRound();
    }
}

public class Net_EndRound : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_EndRound()
    {
        code = OpCode.END_ROUND;
    }

    public Net_EndRound(DataStreamReader reader)
    {
        code = OpCode.END_ROUND;
        Deserialize(reader);
    }

    public Net_EndRound(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.END_ROUND;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.EndRound();
    }
}

public class Net_EndGame : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_EndGame()
    {
        code = OpCode.END_GAME;
    }

    public Net_EndGame(DataStreamReader reader)
    {
        code = OpCode.END_GAME;
        Deserialize(reader);
    }

    public Net_EndGame(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.END_GAME;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.EndGame();
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
