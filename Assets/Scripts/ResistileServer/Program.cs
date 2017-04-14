using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
namespace ResistileServer
{
    class Program
    {
        private static TcpListener serverSocket;
        private static TcpClient clientSocket;
        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        private static bool running = true;
        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            writeServerTitle();
            IPAddress ip = IPAddress.Parse("0.0.0.0");
            serverSocket = new TcpListener(ip, 8888);
            clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (running)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started!");
                HandleClient client = new HandleClient();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            
        }
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                running = false;
                Console.WriteLine("Console window closing, death imminent");
                clientSocket.Close();
                serverSocket.Stop();
                Console.WriteLine(" >> " + "exit");
                
            }
            return false;
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