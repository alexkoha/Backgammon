using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public interface IField
    {
        bool AddStone(PlayerColor color);
        bool SubStone(PlayerColor color);
    }
}
