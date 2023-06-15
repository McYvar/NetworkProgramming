﻿using Unity.Networking.Transport;
using UnityEngine;

public class Net_SpawnPlayer : NetMessage
{
    public int playerId { get; set; }
    public float xPos { get; set; }
    public float yPos { get; set; }
    public float zPos { get; set; }
    private PlayerSpawner playerSpawner;

    public Net_SpawnPlayer()
    {
        code = OpCode.SPAWN_PLAYER;
    }

    public Net_SpawnPlayer(int playerId, float xPos, float yPos, float zPos)
    {
        code = OpCode.SPAWN_PLAYER;
        this.playerId = playerId;
        this.xPos = xPos;
        this.yPos = yPos;
        this.zPos = zPos;
    }

    public Net_SpawnPlayer(DataStreamReader reader)
    {
        code = OpCode.SPAWN_PLAYER;
        Deserialize(reader);
    }

    public Net_SpawnPlayer(DataStreamReader reader, PlayerSpawner playerSpawner)
    {
        code = OpCode.SPAWN_PLAYER;
        this.playerSpawner = playerSpawner;
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
        Debug.Log($"SERVER: {playerId}, {xPos}, {yPos}, {zPos}");
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        Debug.Log($"CLIENT: {playerId}, {xPos}, {yPos}, {zPos}");
        playerSpawner.SpawnPlayer(new Vector3(xPos, yPos, zPos));
    }
}