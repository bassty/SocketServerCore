using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;

namespace SocketServer2
{
    class SocketServer2Programm
    {

        private static TcpListener tcpListener;
        private static int port = 8081;

        static void Main(string[] args)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            Thread listenerThread = new Thread(new ThreadStart(StartListen));
            listenerThread.Start();
        }

        private static void StartListen()
        {
            throw new NotImplementedException();
        }

        //private string GetFileName(string directory)
        //{
        //    StreamReader reader = new StreamReader();
        //    String line;

        //    return fileileName;
        //}

        public void SendOut(String data, ref Socket serverSocket)
        {
            SendOut(Encoding.ASCII.GetBytes(data), ref serverSocket);
        }

        public void SendOut(Byte[] sendDataBytes, ref Socket serverSocket)
        {
            int bytes = 0;

            try
            {
                if (serverSocket.Connected)
                {
                    if ((bytes = serverSocket.Send(sendDataBytes, sendDataBytes.Length,0)) == -1)
                    {
                        Console.WriteLine("Error at reading!");
                    }
                    else
                    {
                        Console.WriteLine("Bytes send {0}", bytes);
                    }
                }
                else
                {
                    Console.WriteLine("Connection failed");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("Error: {0}", e);
            }
        }

        public void Listening()
        {
            Socket serverSocket = tcpListener.AcceptSocket();

            if (serverSocket.Connected)
            {
                Console.WriteLine("Socket Connected");

                byte[] receivedBytes = new Byte[1024];
                int i = serverSocket.Receive(receivedBytes, receivedBytes.Length, 0);
            }
        }
    }
}
