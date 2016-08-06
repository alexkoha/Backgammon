using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public abstract class Player 
    {
        public PlayerType PlayerType { get; }
        public PlayerColor PlayerColor { get; }

        protected Player(PlayerColor color, PlayerType type) 
        {
            PlayerType = type;
            PlayerColor = color;
        }

        public abstract void AskRoll(Controller game); 
        public abstract void AskMove(Controller game);
        public abstract bool MakeMove(Moves move);
        public abstract bool MakeMove(int source , int target );

    }
}
