using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class BaseServer : MonoBehaviour
{
    [SerializeField] private int initialCapacity = 4;
    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

    public ushort port = 9000;
    public string ip = "0.0.0.0";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // init driver
        driver = NetworkDriver.Create();
        //NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        var endpoint = NetworkEndPoint.Parse(ip, port);
        endpoint.Port = port;
        if (driver.Bind(endpoint) != 0) Debug.Log($"Error binding to port: {port}");
        else driver.Listen();
        Debug.Log(endpoint);
        // init connection list
        connections = new NativeList<NetworkConnection>(initialCapacity, Allocator.Persistent);
    }
    private void OnDestroy()
    {
        if (driver.IsCreated)
        {
            driver.Dispose();
            connections.Dispose();
        }
    }
    private void Update()
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

    public virtual void OnData(DataStreamReader stream) { }

    public virtual void BroadCast(NetMessage msg)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i].IsCreated) SendToClient(connections[i], msg);
        }
    }

    public void SendToClient(NetworkConnection connection, NetMessage msg)
    {
        Debug.Log($"trying to send to client {msg.GetType()}");
        driver.BeginSend(connection, out var writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
