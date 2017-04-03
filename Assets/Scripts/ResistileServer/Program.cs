using System;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
namespace ResistileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            writeServerTitle();
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ip, 8888);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                HandleClient client = new HandleClient();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            //clientSocket.Close();
            //serverSocket.Stop();
            //Console.WriteLine(" >> " + "exit");
            //Console.ReadLine();
        }
        private static void writeServerTitle()
        {
            Console.WriteLine();
            Console.WriteLine(@"   ___          _    __  _ __   
  / _ \___ ___ (_)__/ /_(_) /__ 
 / , _/ -_|_-</ (_-< __/ / / -_)
/_/|_|\__/___/_/___|__/_/_/\__/ 
  / __/__ _____  _____ ____     
 _\ \/ -_) __/ |/ / -_) __/     
/___/\__/_/  |___/\__/_/        
                               ");
        }
    }

    //Class to handle each client request separatly

}
