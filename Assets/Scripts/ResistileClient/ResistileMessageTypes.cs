using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileClient
{
    class ResistileMessageTypes
    {
        public const int ping = 0;
        public const int hostWaiting = 1;
        public const int guestRequest = 2;
        public const int guestCancel = 3;
        public const int hostList = 4;
        public const int hostDecline = 5;
        public const int startGame = 6;
        public const int initializeGame = 7;
        public const int validMoves = 8;
        public const int playerTurn = 9;
        public const int newTiles = 10;
        public const int gameResults = 11;
        public const int replay = 12;
        //Server to Client
        public const int initialize = 13;
        public const int host = 14;
        public const int cancelHost = 15;
        public const int start = 16;
        public const int serverList = 17;
        public const int joinLobby = 18;
        public const int cancelJoin = 19;
        public const int gameLoaded = 20;
        public const int rotate = 21;
        public const int endTurn = 22;
        public const int resGuess = 23;
        public const int quitGame = 24;
    }
}
