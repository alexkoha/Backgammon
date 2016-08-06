using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    class FieldOut : FieldBase
    {
        public FieldOut(int num) : base(num)
        {
        }

        public override bool IsAccessebleBy(PlayerColor color)
        {
            return true;
        }
    }
}
