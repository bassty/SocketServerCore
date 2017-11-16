using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientCore
{
    class WebSocketClientProgram
    {

        private static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Client";

            LoopConnect();
            SendLoop();

            Console.Read();
        }

        private static void SendLoop()
        {
            while (true)
            {
                Thread.Sleep(500);
                //Console.Write("Enter a request: ");
                //string req = Console.ReadLine();
                byte[] buffer = Encoding.ASCII.GetBytes("get time");

                clientSocket.Send(buffer);
                byte[] receivedBuf = new byte[1024];
                int rec = clientSocket.Receive(receivedBuf);

                byte[] data = new byte[rec];
                Array.Copy(receivedBuf, data, rec);
                Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));

            }
        }

        private static void LoopConnect()
        {
            int attempts = 0;

            while (!clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    clientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("Connection attempts: " + attempts.ToString());
                } 
            }

            Console.Clear();
            Console.WriteLine("Connected!");
        }
    }
}
