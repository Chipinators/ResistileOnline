using System.Collections;

namespace ResistileClient
{
    public class ResistileMessage
    {
        public int gameID;
        public int messageCode;
        public string message;
        public int tileID;
        public bool turn;

        ResistileMessage()
        {
            gameID = -1;
            messageCode = -1;
            message = "";
        }
        public ResistileMessage(int gameID, int messageCode, string message)
        {
            this.gameID = gameID;
            this.messageCode = messageCode;
            this.message = message;
        }
    }
}