using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;

public class ClientBehaviourExample : MonoBehaviour
{
    public NetworkDriver driver;
    public NetworkConnection connection;
    public bool Done;

    private void Start()
    {
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);

        var enpoint = NetworkEndPoint.Parse("192.168.2.30", 25566);
        connection = driver.Connect(enpoint);
    }

    private void OnDestroy()
    {
        driver.Dispose();
    }

    private void Update()
    {
        driver.ScheduleUpdate().Complete();

        if (!connection.IsCreated)
        {
            if (!Done)
            {
                Debug.Log("Something went wrong during connect");
                return;
            }
        }

        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server");
                uint value = 1;
                int sending = driver.BeginSend(connection, out var writer);
                Debug.Log($"Client sending {sending}");
                writer.WriteUInt(value);
                int endsending = driver.EndSend(writer);
                Debug.Log($"Client endsending {endsending}");
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                uint value = stream.ReadUInt();
                Debug.Log($"Got the value {value} back from the server");
                Done = true;
                connection.Disconnect(driver);
                connection = default(NetworkConnection);
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection = default(NetworkConnection);
            }
        }
    }
}
