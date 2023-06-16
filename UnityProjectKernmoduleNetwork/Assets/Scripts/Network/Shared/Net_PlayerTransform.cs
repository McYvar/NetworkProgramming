using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_PlayerTransform : NetMessage
{
    public int playerId { get; set; }
    public float xPos { get; set; }
    public float yPos { get; set; }
    public float zPos { get; set; }
    public float xRot { get; set; }
    public float yRot { get; set; }
    public float zRot { get; set; }
    private PlayerTransformer playerTransformer;

    public Net_PlayerTransform() 
    {
        code = OpCode.PLAYER_TRANSFORM;
    }

    public Net_PlayerTransform(int playerId, float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
    {
        code = OpCode.PLAYER_TRANSFORM;
        this.playerId = playerId;
        this.xPos = xPos;
        this.yPos = yPos;
        this.zPos = zPos;
        this.xRot = xRot;
        this.yRot = yRot;
        this.zRot = zRot;
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
        writer.WriteFloat(xRot);
        writer.WriteFloat(yRot);
        writer.WriteFloat(zRot);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        playerId = reader.ReadInt();
        xPos = reader.ReadFloat();
        yPos = reader.ReadFloat();
        zPos = reader.ReadFloat();
        xRot = reader.ReadFloat();
        yRot = reader.ReadFloat();
        zRot = reader.ReadFloat();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        Debug.Log($"Server: player with {playerId} to: {xPos}, {yPos}, {zPos}");
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        playerTransformer.TransformPlayer(playerId, xPos, yPos, zPos, xRot, yRot, zRot);
    }
}
