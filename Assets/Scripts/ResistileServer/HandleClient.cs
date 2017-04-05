using System;
using System.Collections;
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
        private string clName;
        //private string targetNo = "0";
        private static List<HandleClient> handleClients = new List<HandleClient>();
        private static List<string> availableHosts = new List<string>();
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
                    handleMessage(message);
                    Console.WriteLine(" >> " + "From client-" + clNo);
                    dataFromClient = message.gameID + " " + message.messageCode + " " + message.message;
                    Console.WriteLine(dataFromClient);
                    networkStream.Flush();

                    
                    if (message.messageCode == 0)
                    {
                        writeClient(1, 1, "Ack");
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
                    Console.WriteLine(" >> " + ex.Message);
                    noException = false;

                }
            }
        }

        private void handleMessage(ResistileMessage message)
        {
            switch (message.messageCode)
            {
                //General
                case ResistileMessageTypes.ping:
                    // reset timeout thing
                    break;
                //Login Scene
                case ResistileMessageTypes.login:
                    // create a player object with username
                    clName = message.message;
                    break;
                //MainMenu
                case ResistileMessageTypes.startHosting:
                    // put player to host list
                    availableHosts.Add(clName);
                    break;
                //Host Scene
                case ResistileMessageTypes.opponentFound:
                    // ????
                    break;
                case ResistileMessageTypes.opponentCanceled:
                    // ????
                    break;
                case ResistileMessageTypes.startGame:
                    break;
                //case ResistileMessageTypes.cancelSearch:
                //    break;
                //case ResistileMessageTypes.declineOpponent:
                //    break;
                //case ResistileMessageTypes.acceptOpponent:
                //    break;
                ////Server Browser
                case ResistileMessageTypes.getHostList:
                    var newMessage = new ResistileMessage(0, ResistileMessageTypes.hostList, "");
                    newMessage.messageArray = new ArrayList(availableHosts.ToArray());
                    writeClient(newMessage);
                    break;
                //case ResistileMessageTypes.hostList:
                //    break;
                //case ResistileMessageTypes.hostDeclined:
                //    break;
                //case ResistileMessageTypes.requestJoinGame:
                //    break;
                //case ResistileMessageTypes.cancelJoinRequest:
                //    break;
                ////In Game
                //case ResistileMessageTypes.initializeGame:
                //    break;
                //case ResistileMessageTypes.tilePlaced:
                //    break;
                //case ResistileMessageTypes.solderPlaced:
                //    break;
                //case ResistileMessageTypes.drawResistor:
                //    break;
                //case ResistileMessageTypes.drawWire:
                //    break;
                //case ResistileMessageTypes.invalidMove:
                //    break;
                //case ResistileMessageTypes.gameResults:
                //    break;
                //case ResistileMessageTypes.replay:
                //    break;
                //case ResistileMessageTypes.opponentQuit:
                //    break;
                //case ResistileMessageTypes.gameLoaded:
                //    break;
                //case ResistileMessageTypes.quitGame:
                //    break;
                //case ResistileMessageTypes.endTurn:
                //    break;
                //case ResistileMessageTypes.rotateTile:
                //    break;
                //case ResistileMessageTypes.guessResistance:
                //    break;
                default:
                    break;
            }
        }

        private void writeClient(string msg)
        {
            string serverResponse = "Server to clinet(" + clNo + ") " + msg;
            var message = new ResistileMessage(0, 0, serverResponse);
            Console.WriteLine("Message sent to client: " + msg);
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

        private void writeClient(int gameID, int msgType, string msg)
        {
            var message = new ResistileMessage(gameID, msgType, msg);
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

        private void writeClient(ResistileMessage msg)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                NetworkStream serverStream = clientSocket.GetStream();
                serializer.Serialize(textWriter, msg);
                var line = textWriter.ToString();
                byte[] outStream = Encoding.ASCII.GetBytes(line + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
        }
    }
}