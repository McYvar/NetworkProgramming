using Unity.Networking.Transport;

public class Net_CloseBarriers : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_CloseBarriers()
    {
        code = OpCode.CLOSE_BARRIERS;
    }

    public Net_CloseBarriers(DataStreamReader reader)
    {
        code = OpCode.CLOSE_BARRIERS;
        Deserialize(reader);
    }
    public Net_CloseBarriers(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.CLOSE_BARRIERS;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.CloseBarriers();
    }
}
