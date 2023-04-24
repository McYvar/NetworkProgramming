using System.Net;
using System.Net.Sockets;
using System.Text;

IPHostEntry ipHostInfo = Dns.GetHostEntry("annie.hku.nl");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 7331);

using Socket client = new(
    ipEndPoint.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

await client.ConnectAsync(ipEndPoint);
InputHandlerAsync();
ReceiveHanlderAsync();

async void InputHandlerAsync()
{
    while (true)
    {
        var message = Console.ReadLine();

        var messageBytes = Encoding.UTF8.GetBytes(message);
        _ = await client.SendAsync(messageBytes, SocketFlags.None);
    }
}

async void ReceiveHanlderAsync()
{

    while (true)
    {
        var buffer = new byte[1_024];
        var received = await client.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, received);
        Console.WriteLine(response);
    }
}

client.Shutdown(SocketShutdown.Both);