using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public interface IController
    {
        bool RegisterMove(Moves move);
        void RollTheDice(Player player);

        void UpdateGameState(object sender, EventArgs e);
    }
}
