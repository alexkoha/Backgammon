using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public interface IGameState
    {
        int MovesToEndFor(PlayerColor color);
        IEnumerable<FieldBase> InilizationFields();
        void SearchPossibleMoves();
    }
}
