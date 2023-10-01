using System.Threading;
using System.Net.Sockets;

namespace Program
{
    class ChatServer
    {
        public static void Connectionhandeler()
        {
            while (true)
            {
                Console.WriteLine("Lokking for connection.");
                Thread.Sleep(5000); //Wait for 5s
            }
        }
        public static void ThreadProc() {
            for (int i = 0; i < 10; i++) {
                Console.WriteLine("ThreadProc: {0}", i);
                // Yield the rest of the time slice.
                Thread.Sleep(1000);
            }
        }

        public static void Main()
        {
            Thread connectionHandeler = new Thread(new ThreadStart(Connectionhandeler));
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
            connectionHandeler.Start();
        }
    }
}