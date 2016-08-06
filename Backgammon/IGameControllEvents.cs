using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public interface IGameControllEvents
    {
        void MakeMove(object sender, EventArgs e);
        void RollDice(object sender, EventArgs e);
        void UpDateScreen(object sender, EventArgs e);
        void GameEnd(object sender, EventArgs e);
        void NoneMoves(object sender, EventArgs e);
    }
}
