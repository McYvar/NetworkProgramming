using Unity.Networking.Transport;

public class Net_PlayerTransform : NetMessage
{
    public int playerId { get; set; }
    public float xPos { get; set; }
    public float yPos { get; set; }
    public float zPos { get; set; }
    private PlayerTransformer playerTransformer;

    public Net_PlayerTransform()
    {
        code = OpCode.PLAYER_TRANSFORM;
    }

    public Net_PlayerTransform(int playerId, float xPos, float yPos, float zPos)
    {
        code = OpCode.PLAYER_TRANSFORM;
        this.playerId = playerId;
        this.xPos = xPos;
        this.yPos = yPos;
        this.zPos = zPos;
    }

    public Net_PlayerTransform(DataStreamReader reader)
    {
        code = OpCode.PLAYER_TRANSFORM;
        Deserialize(reader);
    }
    public Net_PlayerTransform(DataStreamReader reader, PlayerTransformer playerTransformer)
    {
        code = OpCode.PLAYER_TRANSFORM;
        this.playerTransformer = playerTransformer;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(playerId);
        writer.WriteFloat(xPos);
        writer.WriteFloat(yPos);
        writer.WriteFloat(zPos);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
        xPos = reader.ReadFloat();
        yPos = reader.ReadFloat();
        zPos = reader.ReadFloat();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        playerTransformer.TransformPlayer(playerId, xPos, yPos, zPos);
    }
}

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
