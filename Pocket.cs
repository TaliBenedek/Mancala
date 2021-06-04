using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancala
{
    class Pocket
    {
        int stones;

        public Pocket()
        {
            stones = 0;
        }

        public Pocket(int stones)
        {
            this.stones = stones;
        }

        public int GetStones()
        {
            return stones;
        }

        public int PickUpStones()
        {
            int heldStones = stones;
            stones = 0;
            return heldStones;
        }

        public void AddStone()
        {
            stones++;
        }

        public void AddStones(int stones)
        {
            this.stones += stones;
        }        
    }
}
