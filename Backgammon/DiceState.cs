using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class DiceState
    {
        private int[] _possibleLenghtMoves;
        public int[] PossibleLenghtMoves => _possibleLenghtMoves;

        private int _numberOfMoves;
        public int NumberOfMoves => _numberOfMoves;

        public DiceState(Dice dice) 
        {
            if (dice.DiceOne == dice.DiceTwo)
            {
                _possibleLenghtMoves = new int[] {dice.DiceTwo, dice.DiceTwo, dice.DiceTwo, dice.DiceTwo};
                _numberOfMoves = 4;
            }
            else
            {
                _possibleLenghtMoves = new int[] {dice.DiceOne, dice.DiceTwo};
                _numberOfMoves = 2;
            }
        }

        public DiceState(int[] possibleLenghtMoves, int numberOfMoves) 
        {
            _possibleLenghtMoves = possibleLenghtMoves;
            _numberOfMoves = numberOfMoves;
        }

        public DiceState ReducedByOne(int lengthMove)
        {
            if (_possibleLenghtMoves.Length == 0)
                throw new Exception("The DiceState is already empty!");

            var isNotReduced = true;

            int[] newPossibleLenghtMoves = new int[_possibleLenghtMoves.Length - 1];
            int index = 0;
            for (int i = 0; i < _possibleLenghtMoves.Length; ++i)
            {
                if (isNotReduced && lengthMove == _possibleLenghtMoves[i])
                {
                    isNotReduced = false;
                    //continue;
                }
                else
                    newPossibleLenghtMoves[index++] = _possibleLenghtMoves[i];
            }

            if (!isNotReduced)
                return new DiceState(newPossibleLenghtMoves , _numberOfMoves);
            else
                throw new Exception("The DiceState does not contain a number " + lengthMove.ToString() + "!");
        }




    }
}
