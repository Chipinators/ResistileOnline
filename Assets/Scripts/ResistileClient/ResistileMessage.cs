namespace ResistileClient
{
    public class ResistileMessage
    {
        public int gameID;
        public int messageCode;
        public string message;

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