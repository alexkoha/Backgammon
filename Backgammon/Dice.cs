using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class Dice
    {
        private int _diceOne;
        private int _diceTwo;

        public int DiceOne => _diceOne;
        public int DiceTwo => _diceTwo;
        public int Sum => _diceOne + _diceTwo;

        public DiceState GetDiceState() => new DiceState(this);

        public static Dice GetNewDice()
        {
            return new Dice();
        }

        public Dice()
        {
            Random rdm = new Random();
            _diceOne = rdm.Next(6) + 1;
            _diceTwo = rdm.Next(6) + 1;
        }


    }
}
