using System.Collections;

namespace ResistileClient
{
    public class ResistileMessage
    {
        public int gameID;
        public int messageCode;
        public string message;
        public int tileID;
        public int solderId = 0;
        public bool turn;
        public int rotation;
        public ArrayList messageArray;
        public ArrayList coordinates;
        public ArrayList PlayerHand;
        public ArrayList WireHand;
        public double PrimaryObjective;
        public ArrayList secondaryObjectives;
        public double guess;

        ResistileMessage()
        {
            gameID = -1;
            messageCode = -1;
            message = "";
        }
        public ResistileMessage(int gameID, int messageCode)
        {
            this.gameID = gameID;
            this.messageCode = messageCode;
        }
        public ResistileMessage(int gameID, int messageCode, string message)
        {
            this.gameID = gameID;
            this.messageCode = messageCode;
            this.message = message;
        }
    }
}