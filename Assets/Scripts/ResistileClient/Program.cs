using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace ResistileClient
{
    class Program
    {
        static System.Net.Sockets.TcpClient clientSocket = new TcpClient();
        //static NetworkStream serverStream;
        private static XmlSerializer serializer;
        static void Main(string[] args)
        {
            var message = new ResistileMessage(0, 0, "hello world!");
            serializer = new XmlSerializer(typeof(ResistileMessage));

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
            string line2 = "";

            
            while (line2 != "quit")
            {
                using (StringWriter textWriter = new StringWriter())
                {
                    line2 = Console.ReadLine();
                    message.message = line2;
                    NetworkStream serverStream = clientSocket.GetStream();
                    serializer.Serialize(textWriter, message);
                    line = textWriter.ToString();
                    byte[] outStream = Encoding.ASCII.GetBytes(line + "$");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                }
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
