using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ResistileServer
{
    public class HandleClient
    {
        TcpClient clientSocket;
        NetworkStream networkStream;
        string clNo;
        private string targetNo = "0";
        private static List<HandleClient> handleClients = new List<HandleClient>();
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            //Thread ctThread = new Thread(doChat);
            Thread readThread = new Thread(readClient);
            readThread.Start();
            handleClients.Add(this);
        }

        private void readClient()
        {
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient;
            while (true)
            {
                try
                {
                    networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" >> " + "From client-" + clNo);
                    Console.WriteLine(dataFromClient);
                    networkStream.Flush();


                    if (dataFromClient[0] == '0')
                    {
                        targetNo = "0";
                    }


                    for (var index = 0; index < handleClients.Count; index++)
                    {
                        var client = handleClients[index];
                        if (client == this)
                            client.writeClient("Success");
                        else
                            client.writeClient(dataFromClient);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }

        private void writeClient(string msg)
        {
            networkStream = clientSocket.GetStream();
            string serverResponse = "Server to clinet(" + clNo + ") " + msg + "$";
            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }
    }
}