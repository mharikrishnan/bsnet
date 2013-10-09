using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Battleship.Processing
{
    public class BattleshipLogic
    {
        private static Dictionary<string, PlayerData> players = new Dictionary<string, PlayerData>();

        public BattleshipLogic()
        {
            
        }

        public string GetOpponent(string userName)
        {
            var opponent = players.Where(x => x.Value.OpponentUserName == "__WAITING__").FirstOrDefault();

            if (opponent.Key == null)
                return "__WAITING__";
            return opponent.Value.UserName;
        }


        public void NewUserEntered(string userName, string opponentUserName)
        {
            PlayerData pd = new PlayerData();
            pd.UserName = userName;
            pd.OpponentUserName = opponentUserName;
            pd.MyBoard = GetRandomBoard();
            pd.UnhitCount = 15;
            pd.IsMyTurn = false;

            players.Add(userName, pd);

            if (players.ContainsKey(opponentUserName))
            {
                players[opponentUserName].OpponentUserName = userName;
                players[opponentUserName].IsMyTurn = true;

                int gameId = players.Count;
                players[opponentUserName].GameId = gameId;
                players[userName].GameId = gameId;
            }
        }

        public string[,] GetRandomBoard()
        {
            string[,] board = new string[10, 10];

            /* Create board here
             * 
             * W - Water
             * S - Ship
             * H - Hit
             * M - Miss
             * 
            */

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    board[r, c] = "W";
                }
            }

            // Place ships

            int horv = new Random(DateTime.Now.Millisecond).Next(50);
            int radd = 0, cadd = 0;
            if (horv % 2 == 0)
            {
                radd = 1;
                cadd = 0;
            }
            else
            {
                radd = 0;
                cadd = 1;
            }

            // length 5
            int row = new Random(DateTime.Now.Millisecond).Next(6);
            int col = new Random(DateTime.Now.Millisecond).Next(6);
            for (int r = 0; r <  5; r++)
            {
                board[row + (r * radd), col + (r * cadd)] = "S";
            }



            // length 4
            row = new Random(DateTime.Now.Millisecond).Next(7);
            col = new Random(DateTime.Now.Millisecond).Next(7);
            horv = new Random(DateTime.Now.Millisecond).Next(50);
            while (DoesShipCollide(row, col, horv, 4, board))
            {
                row = new Random(DateTime.Now.Millisecond).Next(7);
                col = new Random(DateTime.Now.Millisecond).Next(7);
            }

            for (int r = 0; r < 4; r++)
            {
                board[row + (r * radd), col + (r * cadd)] = "S";
            }

            // length 3
            row = new Random(DateTime.Now.Millisecond).Next(8);
            col = new Random(DateTime.Now.Millisecond).Next(8);
            horv = new Random(DateTime.Now.Millisecond).Next(50);
            while (DoesShipCollide(row, col, horv, 3, board))
            {
                row = new Random(DateTime.Now.Millisecond).Next(8);
                col = new Random(DateTime.Now.Millisecond).Next(8);
            }

            for (int r = 0; r < 3; r++)
            {
                board[row + (r * radd), col + (r * cadd)] = "S";
            }

            // length 3
            row = new Random(DateTime.Now.Millisecond).Next(8);
            col = new Random(DateTime.Now.Millisecond).Next(8);
            horv = new Random(DateTime.Now.Millisecond).Next(50);
            while (DoesShipCollide(row, col, horv, 3, board))
            {
                row = new Random(DateTime.Now.Millisecond).Next(8);
                col = new Random(DateTime.Now.Millisecond).Next(8);
            }

            for (int r = 0; r < 3; r++)
            {
                board[row + (r * radd), col + (r * cadd)] = "S";
            }

            return board;
        }

        private bool DoesShipCollide(int r, int c, int horv, int leng, string[,] board)
        {
            if (horv % 2 == 0)
            {
                for (int chk = 0; chk < leng; chk++)
                {
                    if (board[r + (chk * 1), c + (chk * 0)] == "S")
                        return true;
                }
            }
            else
            {
                for (int chk = 0; chk < leng; chk++)
                {
                    if (board[r + (chk * 0), c + (chk * 1)] == "S")
                        return true;
                }
            }

            return false;
        }

        public bool AllowedToPlay(string userName)
        {
            if (players.ContainsKey(userName))
            {
                return players[userName].IsMyTurn;
            }
            return false;
        }

        public string MakeMove(string userName, string cellID)
        {
            // user + "-cell-" + row + "-" + col eg user-cell-1-2
            int rowStartIndex = cellID.LastIndexOf("l-") + 2;
            int rowEndIndex = cellID.LastIndexOf("-");
            int colStartIndex = rowEndIndex + 1;
            int colEndIndex = cellID.Length;
            int row, col;
            Int32.TryParse(cellID.Substring(rowStartIndex, rowEndIndex - rowStartIndex), out row);
            Int32.TryParse(cellID.Substring(colStartIndex, colEndIndex - colStartIndex), out col);

            if (players.ContainsKey(userName))
            {
                if (players[players[userName].OpponentUserName].MyBoard[row, col] == "S")
                {
                    players[players[userName].OpponentUserName].MyBoard[row, col] = "H";
                    players[players[userName].OpponentUserName].UnhitCount--;
                    return "H";
                }
                else if (players[players[userName].OpponentUserName].MyBoard[row, col] == "W")
                {
                    players[players[userName].OpponentUserName].MyBoard[row, col] = "M";
                    return "M";
                }
            }
            return "ERR";
        }

        public bool DidIWin(string userName)
        {

            if (players.ContainsKey(userName))
            {
                if (players[players[userName].OpponentUserName].UnhitCount == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void GameOver(string userName)
        {
            if (players.ContainsKey(userName))
            {
                players[userName].IsMyTurn = false;
                players[players[userName].OpponentUserName].IsMyTurn = false;
            }
        }

        public void SetNextTurn(string userName)
        {
            if (players.ContainsKey(userName))
            {
                players[userName].IsMyTurn = false;
                players[players[userName].OpponentUserName].IsMyTurn = true;
            }
        }

        public string GetOpponentUser(string userName)
        {
            if (players.ContainsKey(userName))
            {
                return players[userName].OpponentUserName;
            }
            return "";
        }

        public int GetGameID(string userName)
        {
            if (players.ContainsKey(userName))
            {
                return players[userName].GameId;
            }
            return -1;
        }

        public string[,] GetPlayerBoard(string userName)
        {
            if (players.ContainsKey(userName))
            {
                return players[userName].MyBoard;
            }
            return null;
        }
    }
}