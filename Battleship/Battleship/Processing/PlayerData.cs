using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battleship.Processing
{
    public class PlayerData
    {
        private string userName;
        private string opponentUserName;
        private string[,] myBoard;
        private bool isMyTurn;
        private int gameId;
        private int unhitCount;

        public int UnhitCount
        {
            get { return unhitCount; }
            set { unhitCount = value; }
        }

        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        public bool IsMyTurn
        {
            get { return isMyTurn; }
            set { isMyTurn = value; }
        }

        public string[,] MyBoard
        {
            get { return myBoard; }
            set { myBoard = value; }
        }

        public string UserName
        {
          get { return userName; }
          set { userName = value; }
        }


        public string OpponentUserName
        {
            get { return opponentUserName; }
            set { opponentUserName = value; }
        }

        public PlayerData()
        {
        }

    }
}