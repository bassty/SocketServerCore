using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServerCore
{
    class WebSocketServerProgram
    {
        private static byte[] buffer = new byte[1024];
        private static List<Socket> clientSockets = new List<Socket>();
        public static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static TcpListener tcpListener = new TcpListener(IPAddress.Any, 8081);

        private static int httpStartPos = 0;

        public static void SetupServer()
        {
            tcpListener.Start();

            Thread socketThread = new Thread(new ThreadStart(tcpListener.Start));
            socketThread.Start();

            Console.Title = "Server";
            Console.WriteLine("Setting up Server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
            serverSocket.Listen(5);     //5 is the number of Clients to witch the server can listen

            //StartListening();

            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            //tcpListener.BeginAcceptSocket(new AsyncCallback(AcceptCallback), null);
        }

        private static void StartListening()
        {
            serverSocket = tcpListener.AcceptSocket();

            if (serverSocket.Connected)
            {
                Console.WriteLine("Socket Connected");

                byte[] receivedBytes = new Byte[1024];
                int i = serverSocket.Receive(receivedBytes, receivedBytes.Length, 0);

                string sBuffer = Encoding.ASCII.GetString(receivedBytes);

                httpStartPos = sBuffer.IndexOf("HTTP", 1);

                string httpVers = sBuffer.Substring(httpStartPos,0);



            }
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket = serverSocket.EndAccept(AR);
            clientSockets.Add(socket);
            Console.WriteLine("Client connected");
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);

            byte[] dataBuf = new byte[received];
            Array.Copy(buffer, dataBuf, received);

            string text = Encoding.ASCII.GetString(dataBuf);
            Console.WriteLine("Received: " + text);

         
            string response = string.Empty;

            if (text.ToLower() != "get time")
            {
                response = "Invalid Request";
            }
            else
            {
                response = DateTime.Now.ToLongTimeString();
                response = GetResource("index.html");
                
            }

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
        }

        private static string GetResource(string filename)
        {
            try
            {
                using (FileStream filestream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var readFile = File.ReadAllText(filename);
                    return readFile;
                }
            }
            catch
            {
                return null;
            }
        }
    }
    
}
