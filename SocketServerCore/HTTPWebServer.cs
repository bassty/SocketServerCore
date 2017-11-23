using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Resources;
using System.Text;
using System.Threading;

namespace SocketServerCore
{
    class HTTPWebServer
    {
        private static HttpListener httplistener;
        //private static TcpListener tcpListener;
        //private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //private static List<Socket> clientSockets = new List<Socket>();

        //private static byte[] buffer = new byte[1024];

        private static string htmlFile = "index.html";
        private static int port = 8081;

        public static void InitializeServer()
        {
            //create a new listener, who listens at a specific Port
            httplistener = new HttpListener();
            //tcpListener = new TcpListener(IPAddress.Any, 8000);


            WebSocketServerProgram.SetupServer();

            //save a new prefix to the prefix collection at the HttpListener class
            //httplistener.Prefixes.Add(String.Format("http://bastitestmqtt.ddns.net:{0}{1}", 8081, "/"));
            httplistener.Prefixes.Add(String.Format("http://localhost:{0}{1}", port, "/"));

            //start the listener to listen
            try
            {
                httplistener.Start();
                //tcpListener.Start();

                //Thread socketThread = new Thread(new ThreadStart(tcpListener.Start));
                //socketThread.Start();

                //serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));

                //serverSocket.Listen(10);

                

                //create a new Thread to run the server/listener
                Thread serverThread = new Thread(new ThreadStart(httplistener.Start));
                serverThread.Start();

                //if (serverSocket.Connected)
                //{
                //    serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                //    Console.WriteLine("Connected to a Socket!");
                //}
                
            }
            catch (Exception)
            {
                //RestartAsAdmin();
            }

            if (httplistener != null)
            {

                httplistener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null); 
            }
            else
            {
                HttpListener httpListener = new HttpListener();
                httplistener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);
            }

            //Stop Server
            Console.Read();
        }

        //private static void AcceptCallback(IAsyncResult ar)
        //{
        //    //Accept an incoming connection and create a new socket to handle the communication
        //    Socket socket = serverSocket.EndAccept(ar);

        //    //Add Socket to Socket-List
        //    clientSockets.Add(socket);
        //    Console.WriteLine("Client connected");

        //    //Start receiving Data from the socket
        //    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);

        //    //Start to accept incoming sockets
        //    serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        //}

        //private static void ReceiveCallback(IAsyncResult ar)
        //{
            
        //}

        //private static void RestartAsAdmin()
        //{
        //    var startInfo = new ProcessStartInfo("dotnet.exe") { Verb = "runas"};
        //    Process.Start(startInfo);
        //    Environment.Exit(0);
        //}

        private static void ContextReceivedCallback(IAsyncResult ar)
        {
            //create object to store the context of the listener
            HttpListenerContext listenerContext;

            //store the context
            listenerContext = httplistener.EndGetContext(ar);

            //restarting to check the context, for new requests
            httplistener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);

            //print out the context
            Console.WriteLine("Request: {0}", listenerContext.Request.Url.LocalPath);

            //serverSocket = tcpListener.AcceptSocket();

            SendRequest(listenerContext);
        }

        private static void SendRequest(HttpListenerContext listenerContext)
        {
            
            //delete the prefix from the ressource name
            //string filename = listenerContext.Request.Url.LocalPath.Remove(0, "/myApp".Length);
            //string filename = listenerContext.Request.Url.LocalPath;
            string filename = listenerContext.ToString();

            //byte[] bytes;
            string byteStream = "";

            if (filename == "time.cmd")
            {
                byteStream = DateTime.Now.ToString();
            }
            if (!string.IsNullOrEmpty(byteStream))
            {
                byteStream = GetResource(htmlFile);
                //bytes = Encoding.ASCII.GetBytes(byteStream);
            }
            //if(filename == "/index.html")
            else
            {
                byteStream = GetResource(htmlFile);

                //WebSocketServerProgram.serverSocket.Send(Encoding.ASCII.GetBytes(byteStream));

                //if (serverSocket.Connected)
                //{
                //    serverSocket.Send(Encoding.ASCII.GetBytes(byteStream));
                //}
                //else
                //{
                //    Console.WriteLine("Socket not Connected!");
                //}
                
                
                //bytes = GetResource("index.html");
            }


            //if (bytes != null)
            //{
            //    listenerContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
            //    listenerContext.Response.Close();
            //}
            //else
            //{
            //    //WriteError(listenerContext, 404);
            //        return;
            //}

        }

        //private static byte[] GetResource(string filename)
        private static string GetResource(string filename)
        {
            try
            {
                //create a new Stream to read the HTML-file
                //using (FileStream filestream = new FileStream("content/" + filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (FileStream filestream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    //create a byte array to store the bytes from the file
                    //byte[] readBuf = new byte[filestream.Length];

                    //string page = "";

                    var readFile = File.ReadAllText(filename);

                    //foreach (var line in readFile)
                    //{
                    //    page += line;
                    //}
                    //read te bytes of the file into the stream
                    //filestream.Read(readBuf, 0, readBuf.Length);

                    //return the buffer
                    //return readBuf;
                    return readFile;
                }
            }
            catch
            {
                return null;
            }
        }

        //private static void WriteError(HttpListenerContext listenerContext, int statusCode)
        //{
        //    try
        //    {
        //        string description = Resources.ResourceManager.GetString("http" + statusCode.ToString());

        //        byte[] descriptionBytes = Encoding.UTF8.GetBytes(description);

        //        listenerContext.Response.StatusCode = statusCode;
        //        listenerContext.Response.StatusDescription = description;
        //        listenerContext.Response.OutputStream.Write(descriptionBytes, 0, descriptionBytes.Length);
        //        listenerContext.Response.Close();
        //    }
        //    catch
        //    {
        //    }


        //}
    }
}
