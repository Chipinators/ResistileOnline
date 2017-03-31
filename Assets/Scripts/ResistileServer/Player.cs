using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
        hand
        userName
        primaryObjective
        secondaryObjective

        getUsername()
        setUsername()

        getPrimaryObj()
        setPrimaryObj()

        getSecondaryObj()
        setSecondaryObj()

        getHand()
        */

namespace ResistileServer
{
    class Player
    {
        public GameTile[] hand;
        private string userName;
        private double primaryObjective;
        private int[] secondaryObjective = new int[2];
        public string getUsername()
        {
            return userName;
        }
        public void setUsername(string newUserName)
        {
            userName = newUserName;
        }
        public double getPrimaryObj()
        {
            return primaryObjective;
        }

        public void setPrimaryObj(double newPrimary)
        {
            primaryObjective = newPrimary;
        }

        public int[] getSecondaryObj()
        {
            return secondaryObjective;
        }

        public void setSecondaryObj(int[] newSecondary)
        {
            secondaryObjective = newSecondary;
        }

        public int[] getHand()
        {
            int[] temp = new int[5];
            for (int i = 0; i < hand.Length; i++)
            {
                temp[i] = hand[i].id;
            }
            return temp;
        }
    }
}
