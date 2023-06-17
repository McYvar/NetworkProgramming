using Unity.Networking.Transport;

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
