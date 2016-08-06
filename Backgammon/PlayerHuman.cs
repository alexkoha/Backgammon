using System;
using System.Linq;

namespace Backgammon
{
    public class PlayerHuman : Player
    {

        public PlayerHuman(PlayerColor Color) : base(Color, PlayerType.Human){}

        public PlayerHuman(PlayerColor cl, PlayerType type) : base(cl, type) { }

        private Controller _currentGame;

        public override void AskMove(Controller game)
        {
            if ( _currentGame.GameState.PossibleMoves.Length == 0 )
            {
                _currentGame.Events.OnNoneMoves();
                _currentGame.RegisterMove(Moves.EmptyMove(PlayerColor));
            }
            _currentGame.Events.OnMakeMove();
        }

        public override void AskRoll(Controller game)
        {
            _currentGame = game;
            game.CurrentPlayer = this;

            _currentGame.Events.OnRollDice();
        }

        public override bool MakeMove(Moves m)
        {
            return _currentGame.RegisterMove(m);
        }

        public override bool MakeMove(int source, int target)
        {
            if(!(source>=0 && source<24 || source==Constants.BandBlack || source==Constants.BandWhite ))
                throw new IndexOutOfRangeException("Source must be in range of 0-23 .");
           if (!(target >= 0 && target < 24 || target == Constants.OutOfBoard))
                throw new IndexOutOfRangeException("Target must be in ranger of 0-23 or 27.");

            var move = new Moves(source,target,PlayerColor);

            return _currentGame.RegisterMove(move);
        }
    }
}