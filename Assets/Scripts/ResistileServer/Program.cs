using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace ResistileServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            //TcpListener serverSocket = new TcpListener(ip, 8888);
            //TcpClient clientSocket = default(TcpClient);
            //int counter = 0;

            //serverSocket.Start();
            //Console.WriteLine(" >> " + "Server Started");

            //counter = 0;
            //while (true)
            //{
            //    counter += 1;
            //    clientSocket = serverSocket.AcceptTcpClient();
            //    Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
            //    HandleClient client = new HandleClient();
            //    client.startClient(clientSocket, Convert.ToString(counter));
            //}

            //clientSocket.Close();
            //serverSocket.Stop();
            //Console.WriteLine(" >> " + "exit");
            //Console.ReadLine();
            testcase();



        }
        static void testcase()
        {
        DeckManager myManager = new DeckManager(); //Deck Creation test.
        }
    }

    //Class to handle each client request separatly
}
