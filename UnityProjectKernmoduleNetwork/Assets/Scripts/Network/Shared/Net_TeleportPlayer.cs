using Unity.Networking.Transport;
using UnityEngine;

public class Net_TeleportPlayer : NetMessage
{
    public int playerId { get; set; }
    public float xPos { get; set; }
    public float yPos { get; set; }
    public float zPos { get; set; }
    private PlayerTeleporter playerTeleporter;

    public Net_TeleportPlayer()
    {
        code = OpCode.TELEPORT_PLAYER;
    }

    public Net_TeleportPlayer(DataStreamReader reader)
    {
        code = OpCode.TELEPORT_PLAYER;
        Deserialize(reader);
    }

    public Net_TeleportPlayer(DataStreamReader reader, PlayerTeleporter playerTeleporter)
    {
        code = OpCode.TELEPORT_PLAYER;
        Deserialize(reader);
    }

    public Net_TeleportPlayer(int playerId, float xPos, float yPos, float zPos)
    {
        code = OpCode.TELEPORT_PLAYER;
        this.playerId = playerId;
        this.xPos = xPos;
        this.yPos = yPos;
        this.zPos = zPos;
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
        playerTeleporter.TeleportplayerTo(playerId, new Vector3(xPos, yPos, zPos));
    }
}
