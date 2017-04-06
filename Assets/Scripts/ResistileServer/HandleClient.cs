using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public int gameID;
        public string clName;
        //private string targetNo = "0";
        private static List<HandleClient> handleClients = new List<HandleClient>();
        private static List<string> availableHosts = new List<string>();
        private GameManager gameManager;
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
            if (!clientSocket.Connected)
            {
                if (availableHosts.Contains(clName))
                {
                    availableHosts.Remove(clName);
                }
                handleClients.Remove(this);
                clientSocket.Close();
            }
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

                    //for (var index = 0; index < handleClients.Count; index++)
                    //{
                    //    var client = handleClients[index];
                    //    if (client == this)
                    //        client.writeClient("Success");
                    //    else
                    //        client.writeClient(dataFromClient);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.Message);
                    noException = false;
                    if (availableHosts.Contains(clName))
                    {
                        availableHosts.Remove(clName);
                    }
                    handleClients.Remove(this);
                    clientSocket.Close();

                }
            }
        }

        private void handleMessage(ResistileMessage message)
        {
            switch (message.messageCode)
            {
                //General
                case ResistileMessageTypes.ping:
                    //TODO: reset timeout thing
                    writeClient(0, ResistileMessageTypes.ping, "Ack");
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
                case ResistileMessageTypes.cancelSearch:
                    availableHosts.Remove(clName);
                    break;
                case ResistileMessageTypes.declineOpponent:
                    //in message client name
                    var opponentToBeDeclined = handleClients.Find(client => client.clName == message.message);
                    opponentToBeDeclined.writeClient(0, ResistileMessageTypes.hostDeclined, clName);
                    availableHosts.Add(clName);
                    break;
                case ResistileMessageTypes.acceptOpponent:
                    var opponentToBeAccepted = handleClients.Find(client => client.clName == message.message);
                    gameID = generateGameId();
                    opponentToBeAccepted.gameID = gameID;
                    opponentToBeAccepted.writeClient(gameID, ResistileMessageTypes.startGame, "");
                    gameManager = new GameManager(clName, opponentToBeAccepted.clName);
                    opponentToBeAccepted.gameManager = gameManager;
                    break;
                //Server Browser
                case ResistileMessageTypes.getHostList:
                    var newMessage = new ResistileMessage(0, ResistileMessageTypes.hostList, "");
                    newMessage.messageArray = new ArrayList(availableHosts.ToArray());
                    writeClient(newMessage);
                    break;
                case ResistileMessageTypes.requestJoinGame:
                    var theHost = handleClients.Find(client => client.clName == message.message);
                    if (theHost != null)
                        theHost.writeClient(0, ResistileMessageTypes.opponentFound, this.clName);
                    else
                        writeClient(0, ResistileMessageTypes.hostNotFound, message.message);
                    availableHosts.Remove(theHost.clName);
                    break;
                case ResistileMessageTypes.cancelJoinRequest:
                    var theHost2 = handleClients.Find(client => client.clName == message.message);
                    theHost2.writeClient(0, ResistileMessageTypes.opponentCanceled, "");
                    break;
                case ResistileMessageTypes.gameLoaded:
                    var player = gameManager.getPlayer(clName);
                    var playersOpponent = gameManager.getOpponent(clName);
                    var initializeGameMessage = new ResistileMessage(gameID, ResistileMessageTypes.initializeGame, playersOpponent.userName);
                    initializeGameMessage.turn = gameManager.currentTurnPlayer == player;
                    ArrayList playerHand = new ArrayList();
                    foreach (GameTile tile in player.hand)
                    {
                        playerHand.Add(tile.id);
                    }
                    initializeGameMessage.PlayerHand = playerHand;

                   
                    ArrayList wirehand = new ArrayList();
                    foreach (GameTile tile in gameManager.wireHand)
                    {
                        wirehand.Add(tile.id);
                    }
                    initializeGameMessage.WireHand = wirehand;
                    initializeGameMessage.PrimaryObjective = player.primaryObjective;
                    initializeGameMessage.secondaryObjectives = new ArrayList(player.secondaryObjective);
                    writeClient(initializeGameMessage);
                    break;
                
                // In Game
                case ResistileMessageTypes.endTurn:
                    //if invalid move MessageType is ResistileMessageTypes.invalidMove
                    //if valid move MessageType is drawTile to this client, tileplaced to other client. to be written....
                    
                    break;
                //case ResistileMessageTypes.solderPlaced:
                //    break;
                //case ResistileMessageTypes.gameResults:
                //    break;
                //case ResistileMessageTypes.replay:
                //    break;

                //case ResistileMessageTypes.quitGame:
                //    break;

                //case ResistileMessageTypes.rotateTile:
                //    break;
                //case ResistileMessageTypes.guessResistance:
                //    break;
                default:
                    break;
            }
        }

        private static int gameIdCount = 1;
        private static int generateGameId()
        {
            return gameIdCount++;
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