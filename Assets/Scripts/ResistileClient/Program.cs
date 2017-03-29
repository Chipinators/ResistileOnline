using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ResistileClient
{
    class Program
    {
        static System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        static NetworkStream serverStream;

        static void Main(string[] args)
        {
            Console.WriteLine("Client Started");
            try
            {
                clientSocket.Connect("127.0.0.1", 8888);
                Console.WriteLine("Client Socket Program - Server Connected ...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't connect to server.\n" + e);
            }
            
            Thread readDataThread = new Thread(ReadData);
            readDataThread.Start();
            string line = "";
            while (line != "quit")
            {
                NetworkStream serverStream = clientSocket.GetStream();
                line = Console.ReadLine();
                byte[] outStream = Encoding.ASCII.GetBytes(line + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
        }

        private static void ReadData()
        {
            while (true)
            {
                NetworkStream serverStream = clientSocket.GetStream();
                byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(inStream);
                returndata = returndata.Substring(0, returndata.IndexOf("$"));
                Console.WriteLine("Data from Server : " + returndata);
                serverStream.Flush();
            }
        }
    }
}
