using Unity.Networking.Transport;

public class Net_Disconnect : NetMessage
{
    public int playerId { get; set; }
    private PlayerSpawner playerSpawner;

    public Net_Disconnect()
    {
        code = OpCode.PLAYER_DISCONNECT;
    }

    public Net_Disconnect(int playerId)
    {
        code = OpCode.PLAYER_DISCONNECT;
        this.playerId = playerId;
    }

    public Net_Disconnect(DataStreamReader reader)
    {
        code = OpCode.PLAYER_DISCONNECT;
        Deserialize(reader);
    }

    public Net_Disconnect(DataStreamReader reader, PlayerSpawner playerSpawner)
    {
        code = OpCode.PLAYER_DISCONNECT;
        this.playerSpawner = playerSpawner;
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
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        playerSpawner.DespawnPlayer(playerId);
    }
}