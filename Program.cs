using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

class Server
{
    public class Client
    {
        CancellationTokenSource tokenSource = new();
        Thread connectionHandeler;
        public int _id;
        public Socket _socket;

        public Client(int id)
        {
            _id = id;
            _socket = serverSocket.Accept();

            connectionHandeler = new(() => ReciveMessage(tokenSource.Token));
            connectionHandeler.Start();
        }

        public void SendMessage()
        {
            // Process and perform operations on the received data 
            // Sending the message back to the client
            string serverMessage = $"Hello, client-{_id}!";
            byte[] temp2 = Encoding.ASCII.GetBytes(serverMessage);
            _socket.Send(temp2);
        }
        public void ReciveMessage(CancellationToken token)
        {
            // Receiving message from the client
            byte[] temp = new byte[1024];
            int clientBytes = _socket.Receive(temp);
            string clientMessage = Encoding.ASCII.GetString(temp, 0, clientBytes);
            Console.WriteLine($"Received data from client: {clientMessage}");
        }

        ~Client()
        {
            // Closing the connection
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }

    // Setting up the server IP address and port number
    static IPAddress serverAddress = IPAddress.Parse("127.0.0.7");
    static int serverPort = 8000;

    // Creating our server socket
    public static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    

    public static void ConnectionHandeler(CancellationToken token)
    {
        List<Client> clients = new();

        // Binding the socket 
        serverSocket.Bind(new IPEndPoint(serverAddress, serverPort));

        // server listening for incoming connections
        serverSocket.Listen();
        Console.WriteLine("Server is listening for connections");

        while(!token.IsCancellationRequested)
        {
            // Accepting a client connection
            clients.Add(new Client(clients.Count));
            Console.WriteLine($"Client-{clients[clients.Count - 1]._id} connected!");

            //clients[0].ReciveMessage();
        }

        // Close the server socket
        serverSocket.Close();
    }

    public static void Main()
    {
        CancellationTokenSource tokenSource = new();
        Thread connectionHandeler = new(() => ConnectionHandeler(tokenSource.Token));

        connectionHandeler.Start();
    }
}
