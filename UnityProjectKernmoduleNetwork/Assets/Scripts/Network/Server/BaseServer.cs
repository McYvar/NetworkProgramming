using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEditor.Experimental.GraphView;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class BaseServer : MonoBehaviour
{
    [SerializeField] private int initialCapacity = 4;
    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

    private void Start() => Init();
    private void OnDestroy() => Shutdown();
    private void Update() => UpdateServer();

    public string ip = "";
    public ushort port = 9000;

    public virtual void Init()
    {
        DontDestroyOnLoad(this);

        // init driver
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);
        if (driver.Bind(endpoint) != 0) Debug.Log($"Error binding to port: {port}");
        else driver.Listen();

        // init connection list
        connections = new NativeList<NetworkConnection>(initialCapacity, Allocator.Persistent);
    }

    public virtual void Shutdown()
    {
        if (driver.IsCreated)
        {
            driver.Dispose();
            connections.Dispose();
        }
    }

    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete(); // note to self, learn job system...
        CleanupConnections();
        AcceptNewConnections();
        UpdateMessagePump();
    }

    private void CleanupConnections()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections.IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }

    private void AcceptNewConnections()
    {
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");
        }
    }

    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated) continue;
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    OnData(stream);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }

    public virtual void OnData(DataStreamReader stream)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();

        switch (opCode)
        {
            case OpCode.CHAT_MESSAGE:
                msg = new Net_ChatMessage(stream);
                break;
            default:
                Debug.Log("Message recieved had no OpCode");
                break;
        }

        msg.ReceivedOnServer(this);
    }

    public virtual void BroadCast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated) SendToClient(connections[i], msg);
        }
    }

    public virtual void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        driver.BeginSend(connection, out var writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
