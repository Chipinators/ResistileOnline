using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ResistileClient
{
    public class ResistilePlayer
    {
        public ArrayList hand;
        public string userName;
        public double primaryObjective;
        public int[] secondaryObjective;
        private const int MAXHAND = 5;

        public ResistilePlayer()
        {
            
        }

        public ResistilePlayer(string newUserName,ArrayList hand, double newPrimaryObjective, int[] newSecondaryOjective)
        {

            userName = newUserName;
            primaryObjective = newPrimaryObjective;
            secondaryObjective = newSecondaryOjective;

            this.hand = hand;
        }

        public ArrayList getHand()
        {
            return hand;
        }
    }
}
