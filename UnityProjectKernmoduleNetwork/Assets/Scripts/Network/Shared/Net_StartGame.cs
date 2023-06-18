using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_StartGame : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_StartGame()
    {
        code = OpCode.START_GAME;
    }

    public Net_StartGame(DataStreamReader reader)
    {
        code = OpCode.START_GAME;
        Deserialize(reader);
    }

    public Net_StartGame(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.START_GAME;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        deathRunGameLoop.StartGame();
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.StartAttempt();
    }
}
