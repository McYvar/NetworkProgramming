using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_ChatMessage : NetMessage
{
    // 0 - 8 bits reserved for OpCode
    // 8 - 128 bits for string msg
    public FixedString128Bytes chatMessage { get; set; }

    public Net_ChatMessage()
    {
        Code = OpCode.CHAT_MESSAGE;
    }
    public Net_ChatMessage(DataStreamReader reader)
    {
        Code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }

    public Net_ChatMessage(string msg) 
    {
        Code = OpCode.CHAT_MESSAGE;
        chatMessage = msg;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte) Code);
        writer.WriteFixedString128(chatMessage);
    }

    public override void Deserialize(DataStreamReader reader) 
    {
        // First byte handled already
        chatMessage = reader.ReadFixedString128();
    }

    public override void ReceivedOnServer(BaseServer server)
    {
        Debug.Log($"SERVER: {chatMessage}");
        server.BroadCast(this);
    }

    public override void ReceivedOnClient()
    {
        Debug.Log($"CLIENT: {chatMessage}");
    }
}
