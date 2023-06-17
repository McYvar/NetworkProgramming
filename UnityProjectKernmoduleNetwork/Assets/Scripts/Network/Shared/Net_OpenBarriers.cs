using Unity.Networking.Transport;

public class Net_OpenBarriers : NetMessage
{
    private DeathRunGameLoop deathRunGameLoop;
    public Net_OpenBarriers()
    {
        code = OpCode.OPEN_BARRIERS;
    }

    public Net_OpenBarriers(DataStreamReader reader)
    {
        code = OpCode.OPEN_BARRIERS;
        Deserialize(reader);
    }
    public Net_OpenBarriers(DataStreamReader reader, DeathRunGameLoop deathRunGameLoop)
    {
        code = OpCode.OPEN_BARRIERS;
        this.deathRunGameLoop = deathRunGameLoop;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)code);
    }

    public override void ReceivedOnClient()
    {
        deathRunGameLoop.OpenBarriers();
    }
}
