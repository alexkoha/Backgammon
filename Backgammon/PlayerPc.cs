using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class PlayerPc : Player
    {
        public PlayerPc(PlayerColor color) : base(color, PlayerType.PC){ }

        public PlayerPc(PlayerColor cl, PlayerType type) : base(cl, type) { }

        private Controller _currentGame;

        public override void AskMove(Controller game)
        {
            if (_currentGame.GameState.PossibleMoves.Length == 0)
            {
                _currentGame.Events.OnNoneMoves();
                _currentGame.RegisterMove(Moves.EmptyMove(PlayerColor));
            }
            else
            {
                var random = new Random();
                var choose = random.Next(_currentGame.GameState.PossibleMoves.Count());
                MakeMove(_currentGame.GameState.PossibleMoves.ElementAt(choose));
                _currentGame.Events.OnMakeMove();
            }
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
            var move = new Moves(source, target, PlayerColor);
            if (_currentGame.RegisterMove(move))
                return true;

            return false;
        }
    }
}
