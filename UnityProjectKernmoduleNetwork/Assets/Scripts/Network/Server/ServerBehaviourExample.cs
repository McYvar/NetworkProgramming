using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class ServerBehaviourExample : MonoBehaviour
{
    public NetworkDriver driver;
    private NativeList<NetworkConnection> connections;

    private void Start()
    {
        driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;
        if (driver.Bind(endpoint) != 0) Debug.Log($"Failed to bind to port{endpoint.Port}");
        else driver.Listen();

        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
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
        driver.ScheduleUpdate().Complete();

        // Clean up connections
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections.IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = driver.Accept()) != default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < connections.Length; i++)
        {
            if (!connections[i].IsCreated) continue;
            NetworkEvent.Type cmd;
            while ((cmd = driver.PopEventForConnection(connections[i], out stream)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadUInt();
                    Debug.Log($"Got {number} from the Client and adding two to it.");
                    number += 2;

                    int sending = driver.BeginSend(NetworkPipeline.Null, connections[i], out var writer);
                    Debug.Log($"Server sending {sending}");
                    writer.WriteUInt(number);
                    int endsending = driver.EndSend(writer);
                    Debug.Log($"Server endsending {endsending}");
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
