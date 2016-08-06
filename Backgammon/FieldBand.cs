using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class FieldBand : FieldBase 
    {
        private PlayerColor _color;
        public PlayerColor Color => _color;

        public FieldBand(PlayerColor color , int num) : base(num)
        {
            _color = color;
        }

        public override bool IsAccessebleBy(PlayerColor color)
        {
            return (_color==color);
        }

    }
}
