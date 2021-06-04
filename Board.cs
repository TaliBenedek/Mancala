using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancala
{
    class Board
    {
        public static readonly int NR_COLS = 6;
        Pocket[] computerPockets = new Pocket[NR_COLS];
        Pocket[] humanPockets = new Pocket[NR_COLS];

        Pocket computerMancala;
        Pocket humanMancala;
        

        #region  Constructors 
        public Board()
        {
            computerMancala = new Pocket(0);
            humanMancala = new Pocket(0);
            InitializePockets(computerPockets);
            InitializePockets(humanPockets);
          
        }      

        public Board(Board board)
        {
            computerMancala = new Pocket(board.computerMancala.GetStones());
            humanMancala = new Pocket(board.humanMancala.GetStones());
            for(int i = 0; i < NR_COLS; i++)
            {
                computerPockets[i] = new Pocket(board.computerPockets[i].GetStones());
                humanPockets[i] = new Pocket(board.humanPockets[i].GetStones());
            }
        }
        #endregion
        private void InitializePockets(Pocket[] pockets)
        {
            for (int i = 0; i < NR_COLS; i++)
            {
                pockets[i] = new Pocket(4);
            }
        }

        /*----------------------------------------
         * display the current status of the board
         ----------------------------------------*/
        public void ShowBoard()
        {
            
            for(int pocketIndex = 0; pocketIndex < computerPockets.Length; pocketIndex++)
            {
                Console.Write("\t" + computerPockets[pocketIndex].GetStones());
            }
            Console.WriteLine("\n" + computerMancala.GetStones() + "\t\t\t\t\t\t\t" + humanMancala.GetStones());
            for (int pocketIndex = 0; pocketIndex < humanPockets.Length; pocketIndex++)
            {
                Console.Write("\t" + humanPockets[pocketIndex].GetStones());
            }
            Console.WriteLine("\n__________________________________________________________");
        }

        /*-----------------------------------------------------------------
         * make the specified move
         *
         * INPUT:   Player who    - MAX or MIN
         *          int    column - into which column was the next piece dropped
         *          
         * OUTPUT:  Board - new board configuration after the move was made
         ----------------------------------------------------------------*/
        public Board MakeMove(Player player, int column)
        {
            if(player == Player.MAX)
            {                
                MakeMove(computerPockets, computerMancala, computerPockets[column], column, humanPockets, true);
            }
            else
            {
                MakeMove(humanPockets, humanMancala, humanPockets[column], column, computerPockets, false);
            }
            return this;
        }
        private void MakeMove(Pocket[] playerPockets, Pocket playerMancala, Pocket chosenPocket, 
            int column, Pocket[] opponentPockets, bool computer)
        {           
            int stones = chosenPocket.PickUpStones();
            while (stones > 0)
            {
                if (computer)
                {
                    while (stones > 0 && column > 0)
                    {
                        playerPockets[--column].AddStone(); 
                        --stones;
                        LandedInEmptyPocket(stones, playerPockets, playerMancala, opponentPockets, column);
                    }
                    if (stones > 0)
                    {
                        playerMancala.AddStone();
                        --stones;
                    }
                    int opponentColumn = 0;
                    while (stones > 0 && opponentColumn < 5)
                    {
                        opponentPockets[opponentColumn].AddStone(); 
                        --stones;
                        opponentColumn++;
                    }
                }
                else
                {
                    while (stones > 0 && column < 5)
                    {
                        playerPockets[++column].AddStone();
                        --stones;
                        LandedInEmptyPocket(stones, playerPockets, playerMancala, opponentPockets, column);                        
                    }
                    if (stones > 0)
                    {
                        playerMancala.AddStone();
                        --stones;
                    }
                    int opponentColumn = 5;
                    while (stones > 0 && opponentColumn >= 0)
                    {
                        opponentPockets[opponentColumn].AddStone(); 
                        --stones;
                        opponentColumn--;
                    }
                }
            }
        }
        /*-----------------------------------------------------------------
         * Check if last stone landed in empty pocket
         *
         * INPUT:   int stones    - number of stones left
         *          Pocket[] playerPockets - pockets of the current player
         *          Pocket playerMancala - mancala of current player
         *          Pocket[] opponentPockets - pockets of opponent
         *          int    column - into which column was the next piece dropped         
         ----------------------------------------------------------------*/
        private void LandedInEmptyPocket(int stones, Pocket[] playerPockets, Pocket playerMancala, Pocket[] opponentPockets, int column)
        {
            if (stones == 0 && playerPockets[column].GetStones() == 1 && opponentPockets[column].GetStones() > 0)
            {
                stones = playerPockets[column].PickUpStones() + opponentPockets[column].PickUpStones();
                playerMancala.AddStones(stones);
            }
        }
        /*-----------------------------------------------------------------
         *Checks if the column selected is a valid move 
         *ie: if there are stones in the column
         -----------------------------------------------------------------*/
        public bool CanMove(int col, bool computer)
        {
            bool canMove;
            if(computer)
            {
                canMove = CanMove(col, computerPockets);
            }
            else
            {
                canMove = CanMove(col, humanPockets);
            }
            return canMove;
        }

        private bool CanMove(int col, Pocket[] pockets)
        {
            bool canMove = true;
            if(pockets[col].GetStones() == 0)
            {
                canMove = false;
            }
            return canMove;
        }
        /*-----------------------------------------------------------------
        *Checks if the game is over, and if so, adds the stones of the opponent
        *to the mancala of the player who emptied his row        *
        -----------------------------------------------------------------*/
        public bool IsOver()
        {
            bool gameOver = false;
            if(IsEmptyRow(computerPockets))
            {
                CollectOpponentRow(humanPockets, computerMancala);
                gameOver = true;
            }
            else if(IsEmptyRow(humanPockets))
            {
                CollectOpponentRow(computerPockets, humanMancala);
                gameOver = true;
            }
            return gameOver;
        }

        /*-----------------------------------------------------------------
        *Checks if all the pockets in a player's row are empty
        -----------------------------------------------------------------*/
        private bool IsEmptyRow(Pocket[] pockets)
        {
            bool retVal = true;
            foreach (Pocket pocket in pockets)
            {
                if (pocket.GetStones() != 0)
                {
                    retVal = false;
                }
            }
            return retVal;
        }

        /*-----------------------------------------------------------------
        *Checks if a certain player is winning
        *ie: if that player has more stones in their mancala than their 
        *opponent
        -----------------------------------------------------------------*/
        public bool IsWin(Player player)
        {
            bool retVal = false;
            if(player == Player.MAX)
            {
                retVal = computerMancala.GetStones() > humanMancala.GetStones();
            }
            else
            {
                retVal = humanMancala.GetStones() > computerMancala.GetStones();
            }
            return retVal;
        }

        /*-----------------------------------------------------------------
        *Collects all the stones of the opponent and places them in the 
        *player's mancala
        -----------------------------------------------------------------*/
        private void CollectOpponentRow(Pocket[] opponentPockets, Pocket playerMancala)
        {
            int stones = 0;
            foreach (Pocket pocket in opponentPockets)
            {
                stones += pocket.PickUpStones();
            }
            playerMancala.AddStones(stones);
        }

        #region heuristic
        /*----------------------------------------------------------------------
         * assign a value between -48 (min is winning) and 48 (MAX is winning)
         * to the given board position
         ---------------------------------------------------------------------*/
        public int HeuristicValue()
        {
            return computerMancala.GetStones() - humanMancala.GetStones();
        }
        #endregion
    }
}
