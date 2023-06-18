using Unity.Networking.Transport;

public class Net_PlayerDied : NetMessage
{
    public int playerId { get; set; }
    private DeathRunGameLoop deathRunGameLoop;

    public Net_PlayerDied()
    {
        code = OpCode.PLAYER_DIED;
    }

    public Net_PlayerDied(int playerId)
    {
        code = OpCode.PLAYER_DIED;
        this.playerId = playerId;
    }

    public Net_PlayerDied(DataStreamReader reader)
    {
        code = OpCode.PLAYER_DIED;
        Deserialize(reader);
    }

    public Net_PlayerDied(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.PLAYER_DIED;
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
        deathRunGameLoop.PlayerDied(playerId);
    }
}