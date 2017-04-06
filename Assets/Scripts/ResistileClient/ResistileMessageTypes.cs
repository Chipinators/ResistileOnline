using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResistileClient
{
    public class ResistileMessageTypes
    {
        //General
        public const int ping = 0;
        //Login Scene
        public const int login = 1;
        //MainMenu
        public const int startHosting = 2;
        //Host Scene
        public const int opponentFound = 3;
        public const int opponentCanceled = 4;
        public const int startGame = 5;
        public const int cancelSearch = 6;
        public const int declineOpponent = 7;
        public const int acceptOpponent = 8;
        //Server Browser
        public const int getHostList = 9;
        public const int hostList = 10;
        public const int hostDeclined = 11;
        public const int requestJoinGame = 12;
        public const int cancelJoinRequest = 13;
        //In Game
        public const int initializeGame = 14;
        public const int tilePlaced = 15;
        public const int drawTile = 17;
        public const int validMove = 18;
        public const int invalidMove = 19;
        public const int gameResults = 20;
        public const int replay = 21;
        public const int opponentQuit = 22;
        public const int gameLoaded = 23;
        public const int quitGame = 24;
        public const int endTurn = 25;
        public const int rotateTile = 26;
        public const int guessResistance = 27;

        //Exceptional Case
        public const int hostNotFound = 28;
    }
}
