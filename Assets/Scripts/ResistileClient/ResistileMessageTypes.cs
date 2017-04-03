using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileClient
{
    class ResistileMessageTypes
    {
        //Client to Server

        public const int hostWaiting = 1;

        public const int initializeGame = 7;
        public const int validMoves = 8;
        public const int playerTurn = 9;
        public const int newTiles = 10;


        //Server to Client
        public const int initialize = 13;
        public const int host = 14;
        public const int cancelHost = 15;
        public const int start = 16;
        public const int serverList = 17;
        public const int gameLoaded = 20;
        public const int rotate = 21;
        public const int endTurn = 22;
        public const int resGuess = 23;


        //CURRENTLY USED
        public const int ping = 0;
        public const int login = 25;
        public const int opponentFound = 26;
        public const int opponentCanceled = 27;
        public const int tilePlaced = 28;
        public const int draw = 29;
        public const int gameOver = 30;
        public const int replay = 12;
        public const int gameResults = 11;
        public const int hostList = 4;
        public const int hostDecline = 5;
        public const int startGame = 6;
        public const int quitGame = 24;
        public const int joinLobby = 18;
        public const int cancelJoin = 19;


    }
}
