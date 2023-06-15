using Unity.Networking.Transport;
using UnityEngine;

public class GameServer : BaseServer
{
    [SerializeField] private GameObject PlayerPrefab;

    public override void OnData(DataStreamReader stream)
    {
        Debug.Log("Received package on server!");
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream);
                break;
            case OpCode.SPAWN_PLAYER:
                msg = new Net_SpawnPlayer(stream);
                break;
            default:
                Debug.Log("Message recieved had no existing OpCode");
                break;
        }

        msg?.ReceivedOnServer(this);
    }
}
