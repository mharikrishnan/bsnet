using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Battleship.Processing
{
    public class BattleHub : Hub
    {
        private BattleshipLogic battleLogic;

        public BattleHub()
        {
            battleLogic = new BattleshipLogic();
        }

        public void NewUserEntered(string userName, string opponentName)
        {
            battleLogic.NewUserEntered(userName, opponentName);
            Clients.All.sendOpponent(userName, opponentName, battleLogic.GetGameID(userName));
            Clients.All.sendOpponent(opponentName, userName, battleLogic.GetGameID(userName));
        }

        public void GetOpponentName(string userName)
        {
            Clients.Caller.getOpponent(battleLogic.GetOpponent(userName));
        }

        public void TileClicked(string userName, string tileID)
        {
            if (battleLogic.AllowedToPlay(userName))
            {
                string hitOrMiss = battleLogic.MakeMove(userName, tileID);
                if (hitOrMiss != "ERR")
                {
                    if (battleLogic.DidIWin(userName))
                    {
                        battleLogic.GameOver(userName);
                        string statusMsg = "Game over!! " + userName + " wins.";
                        Clients.All.bombFired(battleLogic.GetGameID(userName), tileID, hitOrMiss, statusMsg);
                    }
                    else
                    {
                        battleLogic.SetNextTurn(userName);
                        string statusMsg = battleLogic.GetOpponentUser(userName) + " to play next...";
                        Clients.All.bombFired(battleLogic.GetGameID(userName), tileID, hitOrMiss, statusMsg);
                    }
                }
            }
        }

        public void GetMyBoard(string userName)
        {
            Clients.Caller.displayMyBoard(battleLogic.GetPlayerBoard(userName));
        }
    }
}