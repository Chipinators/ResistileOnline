using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ResistileClient
{
    public class Player
    {
        public ArrayList hand;
        private string userName;
        private double primaryObjective;
        private int[] secondaryObjective = new int[2];
        private const int MAXHAND = 5;

        public Player(string newUserName,ArrayList hand, double newPrimaryObjective, int[] newSecondaryOjective)
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
