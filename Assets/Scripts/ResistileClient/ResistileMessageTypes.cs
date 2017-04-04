using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileClient
{
    class ResistileMessageTypes
    {
        //General
        public const int ping = 0;
        //Login Scene
        public const int login = 1;
        //Host Scene
        public const int startHosting = 2;
        public const int opponentFound = 3;
        public const int opponentCanceled = 4;
        public const int startGame = 5;
        public const int cancelSearch = 6;
        public const int declineOpponent = 7;
        public const int acceptOpponent = 8;
        //Server Browser
        public const int getHostList = 6;
        public const int hostList = 7;
        public const int hostDeclined = 8;
        public const int requestJoinGame = 9;
        public const int cancelJoinRequest = 10;
        //In Game
        public const int initializeGame = 11;
        public const int tilePlaced = 12;
        public const int drawResistor = 13;
        public const int drawWire = 14;
        public const int invalidMove = 15;
        public const int gameResults = 16;
        public const int replay = 17;
        public const int opponentQuit = 18;
        public const int gameLoaded = 19;
        public const int quitGame = 20;
        public const int endTurn = 21;
        public const int rotateTile = 22;
        public const int guessResistance = 23;
    }
}
