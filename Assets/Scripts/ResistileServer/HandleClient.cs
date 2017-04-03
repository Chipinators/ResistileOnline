using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using ResistileClient;

namespace ResistileServer
{

    public class HandleClient
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(ResistileMessage));
        TcpClient clientSocket;
        string clNo;
        //private string targetNo = "0";
        private static List<HandleClient> handleClients = new List<HandleClient>();
        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            Thread readThread = new Thread(readClient);
            readThread.Start();
            handleClients.Add(this);
        }

        private void readClient()
        {
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            string dataFromClient;
            //while timeout datetime greater than current datetime
            bool noException = true;
            while (noException)
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
                    dataFromClient = Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    var bytes = Encoding.ASCII.GetBytes(dataFromClient);
                    ResistileMessage message;
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        using (StreamReader ms = new StreamReader(stream, Encoding.ASCII))
                        {
                            message = (ResistileMessage)serializer.Deserialize(ms);
                        }
                    }

                    Console.WriteLine(" >> " + "From client-" + clNo);
                    dataFromClient = message.gameID + " " + message.messageCode + " " + message.message;
                    Console.WriteLine(dataFromClient);
                    networkStream.Flush();


                    //if (dataFromClient[0] == '0')
                    //{
                    //    targetNo = "0";
                    //}


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
                    Console.WriteLine(" >> " + ex.Message);
                    noException = false;

                }
            }
        }

        private void writeClient(string msg)
        {
            string serverResponse = "Server to clinet(" + clNo + ") " + msg;
            var message = new ResistileMessage(0, 0, serverResponse);
            using (StringWriter textWriter = new StringWriter())
            {
                NetworkStream serverStream = clientSocket.GetStream();
                serializer.Serialize(textWriter, message);
                var line = textWriter.ToString();
                byte[] outStream = Encoding.ASCII.GetBytes(line + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
        }
    }
}