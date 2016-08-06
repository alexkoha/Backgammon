using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class Controller : IController
    {
        public ControllerEvents Events { get; set; }

        private Player _pblack;
        public Player PBlack => _pblack;

        private Player _pwhite;
        public Player PWhite => _pwhite;

        private Dice _dice;
        public Dice Dice => _dice;

        public int BlackOuts => _gameState.Fields[Constants.OutOfBoard].BlackTools;
        public int WhiteOuts => _gameState.Fields[Constants.OutOfBoard].WhiteTools;

        public Player CurrentPlayer;

        private GameState _gameState;

        public GameState GameState
        {
            get { return _gameState; }
            set
            {
                _gameState = value;
                Events.OnUpdateGame();
            }
        }

        public Board GetBoard()
        {
            Board board;
            if (_gameState == null)
                board = new Board(new FieldBase[27], null, new int[0], new Dictionary<int, int[]>());
            else
                board = new Board(_gameState.Fields, _dice, _gameState.PossibleSources, _gameState.PossibleTargets);
            return board;
        }

        public Controller(Player white, Player black , IGameControllEvents gameUI)
        {
            _pblack = black;
            _pwhite = white;
            Events = new ControllerEvents(this, gameUI);
            _gameState = StartNewGameState();
            //Events.OnUpdateGame();

        }

        public Controller(Player white, Player black, GameState gameState)
        {
            _pblack = black;
            _pwhite = white;
            _gameState = gameState;         
        }

        private GameState StartNewGameState()
        {
            return new GameState(StateType.PreGame , PlayerColor.White , GameState.InilizationFields() , null , new PreGame());
        }

        public bool RegisterMove(Moves move)
        {
            if (_gameState.Turn != move.Color || move == null || _gameState.CurrentTurn != TurnType.Move)
                return false;

            if (move.IsEmpty)
                if (_gameState.PossibleMoves.Length == 0)
                {
                    GameState = new GameState(StateType.Real, ChangePlayerColor(GameState.Turn),
                        (FieldBase[])GameState.Fields.Clone(), null, GameState.PreGame);
                    return true;
                }
                else
                    return false;

            if (!IsMoveExcist(move))
                return false;

            var newFields = (FieldBase[])_gameState.Fields.Clone();
            var newTurn = _gameState.Turn;
            var newDiceState = _gameState.DiceState.ReducedByOne(move.Lenght);

            MoveStone(move, newFields);

            if (move.Color == PlayerColor.Black)
            {
                if (newFields[move.Target].WhiteTools == 1)
                {
                    BandStone(move, newFields, PlayerColor.White);
                }
            }
            else
            {
                if (newFields[move.Target].BlackTools == 1)
                {
                    BandStone(move, newFields, PlayerColor.Black);
                }
            }

            if (newDiceState.PossibleLenghtMoves.Length == 0)
            {
                newDiceState = null;
                newTurn = ChangePlayerColor(newTurn);
            }

            GameState = new GameState(StateType.Real, newTurn, newFields, newDiceState, GameState.PreGame);

            return true;
        }

        private bool IsMoveExcist(Moves move)
        {
            foreach (var someMove in _gameState.PossibleMoves)
            {
                if (someMove.Equals(move))
                    return true;
            }
            return false;
        }

        private void BandStone(Moves move, FieldBase[] newFields , PlayerColor color)
        {
            if(color==PlayerColor.White)
                newFields[Constants.BandWhite].AddStone(color);
            else
                newFields[Constants.BandBlack].AddStone(color);

            newFields[move.Target].SubStone(color);
        }

        private void MoveStone(Moves move, FieldBase[] newFields)
        {
            newFields[move.Source].SubStone(move.Color);
            newFields[move.Target].AddStone(move.Color);
        }

        private bool RegisterNewDice()
        {
            if (_gameState.CurrentSaType == StateType.Real) 
            {
                if (_gameState.DiceState != null)
                    return false;

                GameState = new GameState(_gameState.CurrentSaType, _gameState.Turn, 
                    (FieldBase[])_gameState.Fields.Clone(), _dice.GetDiceState() , GameState.PreGame);
                    return true;
            }
            else 
            {
                if (GameState.PreGame.UpdatePreGameDice(GameState.Turn, _dice.Sum))
                {
                    if (GameState.PreGame.PreGameWin() != null)
                    {
                        GameState = new GameState(StateType.Real, (PlayerColor) GameState.PreGame.PreGameWin(),
                            (FieldBase[]) GameState.Fields.Clone(), null, GameState.PreGame);
                    }
                    else
                    {
                        GameState = new GameState(StateType.PreGame, ChangePlayerColor(GameState.Turn),
                            (FieldBase[]) GameState.Fields.Clone(), null , GameState.PreGame);
                    }
                    return true;
                }
                else
                    return false;
            }


        }

        public void RollTheDice(Player player)
        {
            if (GameState.Turn != player.PlayerColor)
                throw new Exception("Its not your turn.");

            _dice = Dice.GetNewDice();

            var seccess = RegisterNewDice();
            if(!seccess)
                throw new Exception("Register New Dices is fail.");
        }

        private PlayerColor ChangePlayerColor(PlayerColor playerColor)
        {
            return playerColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        }

        public PlayerColor? CheckForWinner()
        {
            var isAllWhiteStonesOut = GameState.Fields[Constants.OutOfBoard].WhiteTools == 15;
            var isAllBlackStonesOut = GameState.Fields[Constants.OutOfBoard].BlackTools == 15;

            if (isAllBlackStonesOut)
                return PlayerColor.Black;
            if (isAllWhiteStonesOut)
                return PlayerColor.White;
            return null;
        }

        public void UpdateGameState(object sender, EventArgs e)
        {
            if (CheckForWinner() == null)
            {
                if (GameState.Turn == PlayerColor.White)
                {
                    if (GameState.CurrentTurn == TurnType.Move)
                        PWhite.AskMove(this);
                    else
                        PWhite.AskRoll(this);
                }
                else
                {
                    if (GameState.CurrentTurn == TurnType.Move)
                        PBlack.AskMove(this);
                    else
                        PBlack.AskRoll(this);
                }
            }
            else
            {
                Events.OnEndTheGame();
            }
        }

    }


}
