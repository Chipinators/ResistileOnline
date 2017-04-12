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
        private Object thisLock = new Object();
        private Object thisLock2 = new Object();
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
            lock (thisLock)
            {
                switch (message.messageCode)
                {
                    //General
                    case ResistileMessageTypes.ping:
                        handlePing();
                        break;
                    //Login Scene
                    case ResistileMessageTypes.login:
                        handleLogin(message);
                        break;
                    //MainMenu
                    case ResistileMessageTypes.startHosting:
                        handleStartHosting();
                        break;
                    //Host Scene
                    case ResistileMessageTypes.cancelSearch:
                        handleCancelSearch();
                        break;
                    case ResistileMessageTypes.declineOpponent:
                        handleDeclineOpponent(message);
                        break;
                    case ResistileMessageTypes.acceptOpponent:
                        handleAcceptOpponent(message);
                        break;
                    //Server Browser
                    case ResistileMessageTypes.getHostList:
                        handleGetHostList();
                        break;
                    case ResistileMessageTypes.requestJoinGame:
                        handleRequestJoinGame(message);
                        break;
                    case ResistileMessageTypes.cancelJoinRequest:
                        handleCancelJoinRequest(message);
                        break;
                    case ResistileMessageTypes.gameLoaded:
                        handleGameLoaded();
                        break;

                    // In Game
                    case ResistileMessageTypes.endTurn:
                        handleEndTurn(message);
                        break;
                    case ResistileMessageTypes.guessResistance:
                        handleGuessResistance(message);
                        break;
                    case ResistileMessageTypes.replay:
                        handleReplay(message);
                        break;
                    case ResistileMessageTypes.quitGame:
                        handleQuit(message);
                        break;
                    case ResistileMessageTypes.applicationQuit:
                        if (availableHosts.Contains(clName))
                        {
                            availableHosts.Remove(clName);
                        }
                        handleClients.Remove(this);
                        clientSocket.Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void handleQuit(ResistileMessage message)
        {
            var opponent = handleClients.Find(handle => handle.clName == gameManager.getOpponent(clName).userName);
            opponent.writeClient(gameID, ResistileMessageTypes.opponentQuit);
        }

        private void handleReplay(ResistileMessage message)
        {
            var player = gameManager.getPlayer(clName);
            var opponent = gameManager.getOpponent(clName);
            var opponentHandle = handleClients.Find(handle => handle.clName == opponent.userName);
            if (message.replay == false)
            {
                var messageToSend = new ResistileMessage(gameID, ResistileMessageTypes.replay) {replay = false};
                opponentHandle.writeClient(messageToSend);
            }
            else
            {
                player.replay = message.replay;
                if (player.replay && opponent.replay)
                {
                    //send both replay
                    var messageToSend = new ResistileMessage(gameID, ResistileMessageTypes.replay) { replay = true };
                    writeClient(messageToSend);
                    opponentHandle.writeClient(messageToSend);
                    handleAcceptOpponent(new ResistileMessage(0, ResistileMessageTypes.acceptOpponent, opponent.userName));
                }
            }
        }

        private void handleGuessResistance(ResistileMessage message)
        {
            gameManager.getPlayer(clName).setGuess(message.guess);
            if (gameManager.getPlayer(clName).guessed && gameManager.getOpponent(clName).guessed)
            {
                //TODO: program this
                //wait two players to come here,
                //then calculate primary based on who is closest,
                //secondaries
                //the winner based on most score gained
                //then send both the message
                var sendThisMessage = new ResistileMessage(gameID, ResistileMessageTypes.gameResults);
                sendThisMessage.messageArray = new ArrayList();
                sendThisMessage.messageArray.Add(true); //primaryScore bool
                sendThisMessage.messageArray.Add(true); //secondaryObj1 bool
                sendThisMessage.messageArray.Add(false); // secondaryObj2 bool
                sendThisMessage.messageArray.Add(true); // guessScore bool
                sendThisMessage.win = true;
                writeClient(sendThisMessage);
                sendThisMessage = new ResistileMessage(gameID, ResistileMessageTypes.gameResults);
                var opponentHandle = handleClients.Find(handles => handles.clName == gameManager.getOpponent(clName).userName);
                sendThisMessage.messageArray = new ArrayList();
                sendThisMessage.messageArray.Add(false); //primaryScore bool
                sendThisMessage.messageArray.Add(true); //secondaryObj1 bool
                sendThisMessage.messageArray.Add(false); // secondaryObj2 bool
                sendThisMessage.messageArray.Add(false); // guessScore bool
                sendThisMessage.win = false;
                opponentHandle.writeClient(sendThisMessage);
            }
            //var resistance = gameManager.calculateResistance();
            //var guess = message.guess;
        }

        private void handleEndTurn(ResistileMessage message)
        {
            //if invalid move MessageType is ResistileMessageTypes.invalidMove
            //if valid move MessageType is drawTile to this client, tileplaced to other client. to be written....
            var tile = gameManager.deck.allTiles[message.tileID];
            var tileToBeCheckedClone = tile.Clone();
            for (int i = 0; i < message.rotation; i++)
            {
                tileToBeCheckedClone.Rotate();
            }
            //check if solder placed
            bool validMove = false;
            bool isSolder = message.solderId > 0;
            int[] coords = (int[])message.coordinates.ToArray(typeof(int));

            if (isSolder) //soldermove
            {
                validMove = gameManager.board.IsValidSolder(tileToBeCheckedClone, coords);
            }
            else
            {
                validMove = gameManager.board.IsValidMove(tileToBeCheckedClone, coords);
            }

            if (!validMove)
            {
                writeClient(gameID, ResistileMessageTypes.invalidMove, "");
            }
            else
            {
                var player = gameManager.getPlayer(clName);
                var opponent = gameManager.getOpponent(clName);
                tile = tileToBeCheckedClone;
                GameTile solder;
                bool isGameOver;

                if (isSolder)
                {
                    //AddTileWithSolder
                    solder = gameManager.deck.allTiles[message.solderId];
                    isGameOver = gameManager.AddTileWithSolder(player, tile, solder, coords);
                    writeClientOnValidMove(isGameOver, isSolder, tile, message.rotation, coords, player, opponent, solder);
                }
                else
                {
                    isGameOver = gameManager.AddTile(player, tile, coords);
                    writeClientOnValidMove(isGameOver, isSolder, tile, message.rotation, coords, player, opponent, null);
                }
            }
        }

        private void writeClientOnValidMove(bool isGameOver, bool isSolder, GameTile tile, int rotation, int[] coords, ResistilePlayer player, ResistilePlayer opponent,
            GameTile solder)
        {
            //Sent opponent placed tile data
            var opponentHandle = handleClients.Find(handle => handle.clName == opponent.userName);
            var oppMsg1 = new ResistileMessage(gameID, ResistileMessageTypes.tilePlaced);
            oppMsg1.tileID = tile.id;
            oppMsg1.turn = true;
            oppMsg1.rotation = rotation;
            oppMsg1.coordinates = new ArrayList(coords);
            opponentHandle.writeClient(oppMsg1);

            writeClient(gameID, ResistileMessageTypes.validMove, "");
            if (isGameOver)
            {

                writeClient(gameID, ResistileMessageTypes.gameOver);
                opponentHandle.writeClient(gameID, ResistileMessageTypes.gameOver);
            }
            else
            {
                
                Thread.Sleep(100);
                //Sent player new tile data
                var messageToBeSent = new ResistileMessage(gameID, ResistileMessageTypes.drawTile);
                var card = gameManager.draw(tile, player);
                messageToBeSent.turn = false;
                messageToBeSent.tileID = card.id;
                writeClient(messageToBeSent);

                Thread.Sleep(100);
                if (card.type.Contains("Wire"))
                {
                    var oppMsg2 = new ResistileMessage(gameID, ResistileMessageTypes.drawTile);
                    oppMsg2.turn = true;
                    oppMsg2.tileID = card.id;
                    opponentHandle.writeClient(oppMsg2);
                }
                if (isSolder)
                {
                    var messageToBeSent2 = new ResistileMessage(gameID, ResistileMessageTypes.drawTile);
                    var cardID2 = gameManager.draw(solder, player);
                    messageToBeSent2.tileID = cardID2.id;
                    messageToBeSent2.turn = false;
                    writeClient(messageToBeSent2);
                }
            }
        }

        private void handleGameLoaded()
        {
            var player = gameManager.getPlayer(clName);
            var playersOpponent = gameManager.getOpponent(clName);
            var initializeGameMessage = new ResistileMessage(gameID, ResistileMessageTypes.initializeGame,
                playersOpponent.userName);
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
        }

        private static void handleCancelJoinRequest(ResistileMessage message)
        {
            var theHost = handleClients.Find(client => client.clName == message.message);
            theHost.writeClient(0, ResistileMessageTypes.opponentCanceled, "");
        }

        private void handleRequestJoinGame(ResistileMessage message)
        {
            var theHost = handleClients.Find(client => client.clName == message.message);
            if (theHost != null)
                theHost.writeClient(0, ResistileMessageTypes.opponentFound, this.clName);
            else
                writeClient(0, ResistileMessageTypes.hostNotFound, message.message);
            availableHosts.Remove(theHost.clName);
        }

        private void handleGetHostList()
        {
            var newMessage = new ResistileMessage(0, ResistileMessageTypes.hostList, "");
            newMessage.messageArray = new ArrayList(availableHosts.ToArray());
            writeClient(newMessage);
        }

        private void handleAcceptOpponent(ResistileMessage message)
        {
            var opponentToBeAccepted = handleClients.Find(client => client.clName == message.message);
            gameID = generateGameId();
            opponentToBeAccepted.gameID = gameID;
            opponentToBeAccepted.writeClient(gameID, ResistileMessageTypes.startGame, "");
            gameManager = new GameManager(clName, opponentToBeAccepted.clName);
            opponentToBeAccepted.gameManager = gameManager;
        }

        private void handleDeclineOpponent(ResistileMessage message)
        {
            //in message client name
            var opponentToBeDeclined = handleClients.Find(client => client.clName == message.message);
            opponentToBeDeclined.writeClient(0, ResistileMessageTypes.hostDeclined, clName);
            availableHosts.Add(clName);
        }

        private void handleCancelSearch()
        {
            availableHosts.Remove(clName);
        }

        private void handleStartHosting()
        {
            // put player to host list
            availableHosts.Add(clName);
        }

        private void handleLogin(ResistileMessage message)
        {
            ResistileMessage messageToSend = new ResistileMessage(gameID, ResistileMessageTypes.login);
            // create a player object with username
            if (handleClients.Exists(handle => handle.clName == message.message))
            {
                messageToSend.turn = false;
            }
            else
            {
                messageToSend.turn = true;
                clName = message.message;
            }
            writeClient(messageToSend);
        }

        private void handlePing()
        {
            //TODO: reset timeout thing
            writeClient(0, ResistileMessageTypes.ping, "Ack");
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
            writeClient(message);
        }

        private void writeClient(int gameID, int msgType, string msg)
        {
            var message = new ResistileMessage(gameID, msgType, msg);
            writeClient(message);
        }

        private void writeClient(int gameID, int msgType)
        {
            var message = new ResistileMessage(gameID, msgType);
            writeClient(message);
        }

        private void writeClient(ResistileMessage msg)
        {
            lock (thisLock2)
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
}