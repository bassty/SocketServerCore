using System;
using System.Collections.Generic;
using System.Text;

namespace SocketServerCore
{
    class ClassMain
    {
        static void Main(string[] args)
        {


            WebSocketServerProgram.SetupServer();
            //HTTPWebServer.InitializeServer();


            Console.Read();
        }


    }
}
