using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class FieldBase : IField
    {
        public int BlackTools;
        public int WhiteTools;
        public int Number { get; }

        public FieldBase(int num)
        {
            BlackTools = 0;
            WhiteTools = 0;

            Number = num;
        }

        public int ToolsSum => BlackTools + WhiteTools;

        public int NumToolsColor(PlayerColor color)
        {
            return color == PlayerColor.White ? WhiteTools : BlackTools;
        }

        public bool AddStone(PlayerColor color)
        {
            if (!IsAccessebleBy(color)) return false;

            if (color == PlayerColor.White) WhiteTools ++;
            else BlackTools++;

            return true; 
        }

        public bool SubStone(PlayerColor color)
        {
            if (color == PlayerColor.Black)
            {
                if (BlackTools <= 0) return false;
                BlackTools--;
            }
            if (color == PlayerColor.White)
            {
                if (WhiteTools <= 0) return false;
                WhiteTools--;
            }
            return true;
        }

        public virtual bool IsAccessebleBy(PlayerColor color) 
        {
            if (color == PlayerColor.Black)
            {
                return !(WhiteTools > 1);
            }
            else
            {
                return !(BlackTools > 1);
            }
        }
    }


}
