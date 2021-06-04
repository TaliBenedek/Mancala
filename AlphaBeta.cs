using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancala
{
    class AlphaBeta
    {
        public static int NrEntries = 0;
        public static double Value(Board board, int depth, double alfa, double beta, Player player)
        {
            Trace.println("Enter alphabeta d = " + depth + " a = " + alfa + " b = " + beta + " P = " + player, 5);
            Player opponent = player == Player.MAX ? Player.MIN : Player.MAX;
            ++NrEntries;
            double value = 0.0;
            if (depth == 0)
            {
                value = board.HeuristicValue();
            }
            else if (board.IsWin(opponent))
            {
                value = player == Player.MAX ? Program.MIN_VALUE : Program.MAX_VALUE;
            }
            else
            {
                if (player == Player.MAX)
                {
                    bool computer = true;
                    for (int col = 0; col < Board.NR_COLS; ++col)
                    {
                        if (board.CanMove(col, computer))
                        {
                            Board tempBoard = new Board(board);
                            Board nextPos = tempBoard.MakeMove(Player.MAX, col);
                            double thisVal = Value(nextPos, depth - 1, alfa, beta, opponent);
                            if (thisVal > alfa)
                            {
                                alfa = thisVal;
                            }
                            if (beta <= alfa)
                            {
                                break;
                            }
                        }
                    }
                    value = alfa;
                }
                else  // player == Player.MIN
                {
                    bool computer = false;
                    for (int col = 0; col < Board.NR_COLS; ++col)
                    {
                        if (board.CanMove(col, computer))
                        {
                            Board tempBoard = new Board(board);
                            Board nextPos = tempBoard.MakeMove(Player.MIN, col);
                            double thisVal = Value(nextPos, depth - 1, alfa, beta, opponent);
                            if (thisVal < beta)
                            {
                                beta = thisVal;
                            }
                            if (beta <= alfa)
                            {
                                break;
                            }
                        }
                    }
                    value = beta;
                }
            }
            Trace.println("Exit alfabeta value = " + value + " depth " + depth, 5);
            return value;
        }
    }
}
