using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SocketServerCore
{
    class HTTPWebServer
    {
        private static HttpListener httplistener;

        private static void InitializeServer()
        {
            //create a new listener, who listens at a specific Port
            httplistener = new HttpListener();

            //save a new prefix to the prefix collection at the HttpListener class
            httplistener.Prefixes.Add(String.Format("http://*:{0}{1}", 8081, "/myApp"));

            //start the listener to listen
            httplistener.Start();

            httplistener.BeginGetContext(new AsyncCallback(ContextReceivedCallback), null);

            //Stop Server
            Console.Read();
        }

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
        }

        private static void SendRequest(HttpListenerContext listenerContext)
        {
            //delete the prefix from the ressource name
            string filename = listenerContext.Request.Url.LocalPath.Remove(0, "/myApp".Length);

            byte[] bytes;
            string byteStream = "";

            if (filename == "time.cmd")
            {
                byteStream = DateTime.Now.ToString();
            }
            if (!string.IsNullOrEmpty(byteStream))
            {
                bytes = Encoding.ASCII.GetBytes(byteStream);
            }
            else
            {
                bytes = GetResource(filename);
            }


            if (bytes != null)
            {
                listenerContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
                listenerContext.Response.Close();
            }
            else
            {
                WriteError(listenerContext, 404);
                    return;
            }

        }

        private static void WriteError(HttpListenerContext listenerContext, int v)
        {
            throw new NotImplementedException();
        }

        private static byte[] GetResource(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
