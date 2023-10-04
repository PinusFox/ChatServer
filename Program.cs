using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

class Server
{
    public static void ConnectionHandeler(CancellationToken token)
    {
        // Setting up the server IP address and port number
        IPAddress serverAddress = IPAddress.Parse("127.0.0.7");
        int serverPort = 8000;

        // Creating our server socket
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        // Binding the socket 
        serverSocket.Bind(new IPEndPoint(serverAddress, serverPort));

        // server listening for incoming connections
        serverSocket.Listen(5);
        Console.WriteLine("Server is listening for connections");

        while(true)
        {
            // Accepting a client connection
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("Client connected!");

            // Receiving message from the client
            byte[] temp = new byte[1024];
            int clientBytes = clientSocket.Receive(temp);
            string clientMessage = Encoding.ASCII.GetString(temp, 0, clientBytes);
            Console.WriteLine("Received data from client: " + clientMessage);

            // Process and perform operations on the received data 
            // Sending the message back to the client
            string serverMessage = "Hello, client!";
            byte[] temp2 = Encoding.ASCII.GetBytes(serverMessage);
            clientSocket.Send(temp2);

             // Closing the connection
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        // Close the server socket
        serverSocket.Close();

        while (!token.IsCancellationRequested)
        {
            Console.WriteLine("Lokking for connection.");
            Thread.Sleep(2000); //Wait for 2s
        }
    }
    public static void Test(CancellationToken token)
    {
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine(i);
            Thread.Sleep(1000);
        }
    }

    public static void Main()
    {
        CancellationTokenSource tokenSource = new();
        Thread connectionHandeler = new(() => ConnectionHandeler(tokenSource.Token));

        connectionHandeler.Start();
    }
}
