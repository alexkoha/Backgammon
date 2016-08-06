using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class PreGame
    {
        private int _diceSumWhite;
        private int _diceSumBlack;

        public PreGame()
        {
            _diceSumBlack = 0;
            _diceSumWhite = 0;
        }

        public bool UpdatePreGameDice(PlayerColor color , int sum)
        {
            if (PreGameWin() != null) return false;

            if (color == PlayerColor.White)
                _diceSumWhite = sum;
            else
            {
                _diceSumBlack = sum;

                if (_diceSumWhite != _diceSumBlack) return true;
                _diceSumWhite = 0;
                _diceSumBlack = 0;
            }
            return true;
        }

        public PlayerColor? PreGameWin()
        {
            if (_diceSumWhite != 0 && _diceSumBlack != 0)
            {
                if (_diceSumWhite > _diceSumBlack)
                    return PlayerColor.White;
                if (_diceSumWhite < _diceSumBlack)
                    return PlayerColor.Black;
            }
            
            return null;
        } 
    }
}
