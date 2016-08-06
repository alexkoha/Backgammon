using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class ControllerEvents
    {
        public ControllerEvents(Controller controlGame, IGameControllEvents gameUI)
        {
            UpdateGame += controlGame.UpdateGameState;
            MakeMove += gameUI.MakeMove;
            RollDice += gameUI.RollDice;
            UpDateScreen += gameUI.UpDateScreen;
            EndTheGame += gameUI.GameEnd;
            NoneMoves += gameUI.NoneMoves;
        }

        private event EventHandler UpdateGame;
        private event EventHandler MakeMove;
        private event EventHandler RollDice;
        private event EventHandler UpDateScreen;
        private event EventHandler StartNewGame;
        private event EventHandler EndTheGame;
        private event EventHandler NoneMoves;

        public void OnUpdateGame()
        {
            UpdateGame?.Invoke(this, null);
        }
        
        internal void OnMakeMove()
        {
            MakeMove?.Invoke(this, EventArgs.Empty);
        }

        internal void OnRollDice()
        {
            RollDice?.Invoke(this, EventArgs.Empty);
        }

        public void OnUpDateScreen()
        {
            UpDateScreen?.Invoke(this, EventArgs.Empty);
        }

        internal void OnStartNewGame()
        {
            StartNewGame?.Invoke(this, EventArgs.Empty);
        }

        internal void OnEndTheGame()
        {
            EndTheGame?.Invoke(this, EventArgs.Empty);
        }

        internal virtual void OnNoneMoves()
        {
            NoneMoves?.Invoke(this, EventArgs.Empty);
        }
    }
}
