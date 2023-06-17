using Unity.Networking.Transport;

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

public class Net_ReachedCheckpoint : NetMessage
{
    public int playerId { get; set; }
    public int checkpointId { get; set; }
    private DeathRunGameLoop deathRunGameLoop;
    public Net_ReachedCheckpoint()
    {
        code = OpCode.REACHED_CHECKPOINT;
    }

    public Net_ReachedCheckpoint(int playerId, int checkpointId)
    {
        code = OpCode.REACHED_CHECKPOINT;
        this.playerId = playerId;
        this.checkpointId = checkpointId;
    }

    public Net_ReachedCheckpoint(DataStreamReader reader)
    {
        code = OpCode.REACHED_CHECKPOINT;
        Deserialize(reader);
    }

    public Net_ReachedCheckpoint(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.REACHED_CHECKPOINT;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
        writer.WriteInt(checkpointId);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
        checkpointId = reader.ReadInt();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.ReachedCheckpoint(playerId , checkpointId);
    }
}
