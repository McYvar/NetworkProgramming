using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class BaseClient : MonoBehaviour
{
    public NetworkDriver driver;
    protected NetworkConnection connection;

    public string ip = "127.0.0.1";
    public ushort port = 9000;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        // init driver
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);

        var endpoint = NetworkEndPoint.Parse(ip, port); 
        endpoint.Port = port;
        connection = driver.Connect(endpoint);
    }

    private void OnDestroy()
    {
        driver.Dispose();
    }

    private void Update()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }

    private void CheckAlive()
    {
        if (!connection.IsCreated)
        {
            Debug.Log("Something went wrong, lost connection to server");
        }
    }

    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;

        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                OnData(stream);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
            }
        }
    }

    public virtual void OnData(DataStreamReader stream)
    {
    }

    public virtual void SendToServer(NetMessage msg)
    {
        driver.BeginSend(connection, out var writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }
}
