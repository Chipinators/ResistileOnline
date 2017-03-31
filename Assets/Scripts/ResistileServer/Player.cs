using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
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



        //public string getUsername()
        //{
        //    return userName;
        //}
        //public void setUsername(string newUserName)
        //{
        //    userName = newUserName;
        //}
        //public double getPrimaryObj()
        //{
        //    return primaryObjective;
        //}

        //public void setPrimaryObj(double newPrimary)
        //{
        //    primaryObjective = newPrimary;
        //}

        //public int[] getSecondaryObj()
        //{
        //    return secondaryObjective;
        //}

        //public void setSecondaryObj(int[] newSecondary)
        //{
        //    secondaryObjective = newSecondary;
        //}
    }
}
