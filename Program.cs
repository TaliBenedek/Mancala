using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancala
{
    class Program
    {
        static int MAX_DEPTH = 4;  // default value reset inside ProcessConfiguration
        static bool ALPHA_BETA = false;
        public static readonly double MIN_VALUE = -48.0;
        public static readonly double MAX_VALUE = 48.0;

        static void Main(string[] args)
        {
            ProcessConfiguration();
            Console.WriteLine("This is a mancala game. The rules are the same as usual, except you don't go again if you land in your mancala. " +
               "\nLet's play!");
            Board gameBoard = new Board();
            gameBoard.ShowBoard();
            while (!gameBoard.IsOver())
            {
                Console.WriteLine("I am thinking about my move now");
                double highVal = -1.0;
                int bestMove = 0;
                double alfa = -1.0;
                double beta = 1.0;
                bool computer = true;
                for (int col = 0; col < Board.NR_COLS; ++col)
                {
                    if (gameBoard.CanMove(col, computer))
                    {
                        Board tempBoard = new Board(gameBoard);
                        Board nextPos = tempBoard.MakeMove(Player.MAX, col);

                        double thisVal = ALPHA_BETA
                            ? AlphaBeta.Value(nextPos, MAX_DEPTH - 1, alfa, beta, Player.MIN)
                            : MiniMax.Value(nextPos, MAX_DEPTH - 1, Player.MIN);
                        Trace.println($" col = {col}   value = {Math.Round(thisVal, 2)}", 11, MAX_DEPTH);
                        if (thisVal > highVal)
                        {
                            bestMove = col;
                            highVal = thisVal;
                        }
                    }
                }
                if (highVal == -1)
                {
                    bestMove = DesperationMove(gameBoard, computer);
                }
                Console.WriteLine($"My move is {(bestMove + 1)}    (subj. value {highVal})");
                gameBoard = gameBoard.MakeMove(Player.MAX, bestMove);
                gameBoard.ShowBoard();

                if (gameBoard.IsOver() && gameBoard.IsWin(Player.MAX))
                {
                    Console.WriteLine("\n I win");
                    gameBoard.ShowBoard();
                }
                else
                {
                    computer = false;
                    Console.WriteLine("\nYour move");
                    int theirMove = UserInput.getInteger("Select column 1 - 6", 1, 6) - 1;
                    bool canMove = gameBoard.CanMove(theirMove, computer);
                    while (!canMove)
                    {
                        Console.WriteLine("There are no stones in that pocket, please choose another one.");
                        theirMove = UserInput.getInteger("Select column 1 - 6", 1, 6) - 1;
                        canMove = gameBoard.CanMove(theirMove, computer);
                    }
                    gameBoard = gameBoard.MakeMove(Player.MIN, theirMove);
                    Console.WriteLine("");
                    gameBoard.ShowBoard();

                    if (gameBoard.IsOver() && gameBoard.IsWin(Player.MIN))
                    {
                        Console.WriteLine("\n You win :-(");
                        gameBoard.ShowBoard();
                    }
                }
            }
            Console.WriteLine("nr of calls to minimax: " + MiniMax.NrEntries);
            Console.WriteLine("nr of calls to AlphaBeta: " + AlphaBeta.NrEntries);
            Console.ReadKey();
        }

        private static int DesperationMove(Board gameBoard, bool computer)
        {
            int ColumnPicked = 0;
            for (int col = 0; col < Board.NR_COLS; ++col)
            {
                if (gameBoard.CanMove(col, computer))
                {
                    Board tempBoard = new Board(gameBoard);
                    Board nextPos = tempBoard.MakeMove(Player.MIN, col);

                    if (nextPos.IsWin(Player.MIN))
                    {
                        ColumnPicked = col;
                        break;
                    }
                }
            }
            return ColumnPicked;
        }

        private static void ProcessConfiguration()
        {

            String strDepth = ConfigurationManager.AppSettings["Depth"];
            var depth = 0;
            if (int.TryParse(strDepth, out depth))
            {
                if (depth > 1 && depth < 10) MAX_DEPTH = depth;
            }

            String strTrace = ConfigurationManager.AppSettings["Trace"];
            Int16 trcVal = 0;
            if (Int16.TryParse(strTrace, out trcVal))
            {
                Trace.ON = true;
                Trace.TraceDetailLevel = trcVal;
            }
            else Trace.ON = false;


            String strAB = ConfigurationManager.AppSettings["AlphaBeta"];
            ALPHA_BETA = strAB.CompareTo("AB") == 0;
        }
    }
}
