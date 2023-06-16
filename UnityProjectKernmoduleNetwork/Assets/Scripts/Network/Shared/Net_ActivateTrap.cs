using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_ActivateTrap : NetMessage
{
    public int trapId;
    private TrapsHandler trapsHandler;

    public Net_ActivateTrap()
    {
        code = OpCode.ACTIVATE_TRAP;
    }

    public Net_ActivateTrap(int trapId)
    {
        code = OpCode.ACTIVATE_TRAP;
        this.trapId = trapId;
    }

    public Net_ActivateTrap(DataStreamReader reader)
    {
        code = OpCode.ACTIVATE_TRAP;
        Deserialize(reader);
    }

    public Net_ActivateTrap(DataStreamReader reader, TrapsHandler trapsHandler)
    {
        code = OpCode.ACTIVATE_TRAP;
        this.trapsHandler = trapsHandler;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
        writer.WriteInt(trapId);
    }

    public override void Deserialize(DataStreamReader reader)
    {
        trapId = reader.ReadInt();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        trapsHandler.ActivateTrap(trapId);
    }
}
