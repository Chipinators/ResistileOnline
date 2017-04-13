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
        public double guess;
        public bool guessed = false;
        public bool replay = false;
        public bool primaryAchieved = false;
        public bool guessAchieved = false;
        public bool[] secondaryObjectiveChecks = new bool[6];
        public bool won = false;

        public ResistilePlayer()
        {
            
        }

        public ResistilePlayer(string newUserName,ArrayList hand, double newPrimaryObjective, int[] newSecondaryOjective)
        {
            Array.Clear(secondaryObjectiveChecks, 0, secondaryObjectiveChecks.Length);
            userName = newUserName;
            primaryObjective = newPrimaryObjective;
            secondaryObjective = newSecondaryOjective;

            this.hand = hand;
        }

        public void setGuess(double guess)
        {
            guessed = true;
            this.guess = guess;
        }

        public ArrayList getHand()
        {
            return hand;
        }
        
        public bool[] getSecondaryObjectiveResults()
        {
            return new[]
                {secondaryObjectiveChecks[secondaryObjective[0]], secondaryObjectiveChecks[secondaryObjective[1]]};
        }
    }
}
