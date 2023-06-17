using Unity.Networking.Transport;

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