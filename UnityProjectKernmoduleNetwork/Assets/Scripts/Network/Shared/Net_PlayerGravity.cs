using Unity.Networking.Transport;

public class Net_PlayerGravity : NetMessage
{
    public int playerId { get; set; }
    public float xDir { get; set; }
    public float yDir { get; set; }
    public float zDir { get; set; }
    private PlayerRotator playerRotator;

    public Net_PlayerGravity()
    {
        code = OpCode.PLAYER_GRAVITY;
    }

    public Net_PlayerGravity(int playerId, float xDir, float yDir, float zDir)
    {
        code = OpCode.PLAYER_GRAVITY;
        this.playerId = playerId;
        this.xDir = xDir;
        this.yDir = yDir;
        this.zDir = zDir;
    }

    public Net_PlayerGravity(DataStreamReader reader)
    {
        code = OpCode.PLAYER_GRAVITY;
        Deserialize(reader);
    }

    public Net_PlayerGravity(DataStreamReader reader, PlayerRotator playerRotator)
    {
        code = OpCode.PLAYER_GRAVITY;
        this.playerRotator = playerRotator;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
        writer.WriteFloat(xDir);
        writer.WriteFloat(yDir);
        writer.WriteFloat(zDir);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
        xDir = reader.ReadFloat();
        yDir = reader.ReadFloat();
        zDir = reader.ReadFloat();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        playerRotator.RotatePlayer(playerId, xDir, yDir, zDir);
    }
}
