using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{ 
    public class GameState //: IGameState
    {
        private FieldBase[] _fields; 
        public FieldBase[] Fields => _fields;

        private PlayerColor _turn;
        public PlayerColor Turn => _turn;

        private DiceState _diceState;
        public DiceState DiceState => _diceState;

        private Moves[] _possibleMoves;
        public Moves[] PossibleMoves => _possibleMoves;

        public Dictionary<int, int[]> PossibleTargets => _possibleTargets;
        private Dictionary<int, int[]> _possibleTargets;

        private int[] _possibleSources;
        public int[] PossibleSources => _possibleSources;

        private StateType _currentStateType;
        public StateType CurrentSaType => _currentStateType;

        private PreGame _preGame;
        public PreGame PreGame => _preGame;

        public GameState(StateType _stateType , PlayerColor turn, FieldBase[] fields, DiceState diceState, PreGame preGame)
        {
            _fields = fields;
            _diceState = diceState;
            _turn = turn;
            _preGame = preGame;
            _currentStateType = _stateType;
            SearchPossibleMoves(); 
        }

        private bool IsBlackInHome()
        {
            if (IsBlackStonesBanded())
                return false;
            for (int i = 6; i < Constants.FieldLenght; i++)
            {
                if (_fields[i].BlackTools > 0)
                    return false;
            }
            return true;
        }

        private bool IsWhiteInHome() 
        {
            if (IsWhiteStonesBanded())
                return false;
            for (int i = 0; i < 18; i++)
            {
                if (_fields[i].WhiteTools > 0)
                    return false;
            }
            return true;
        }

        private bool IsWhiteStonesBanded()
        {
            return _fields[Constants.BandWhite].WhiteTools > 0;
        }

        private bool IsBlackStonesBanded()
        {
            return _fields[Constants.BandBlack].BlackTools > 0;
        }

        public bool IsBandedStones(Player player)
        {
            return player.PlayerColor == PlayerColor.White ? IsWhiteStonesBanded() : IsBlackStonesBanded();
        }

        private IEnumerable<Moves> SearchAllPossibleMoves(PlayerColor color)
        {
            var movesHandler = new HashSet<Moves>();
            if (color == PlayerColor.White)
            {
                if (!IsWhiteStonesBanded())
                {
                    if (IsWhiteInHome()) 
                    {
                        foreach (var pipe in _diceState.PossibleLenghtMoves)
                        {
                            int startSource = Constants.FieldLenght - pipe;
                            for (int source = startSource; source < Constants.FieldLenght; source++)
                            {
                                if (source > 0 && _fields[source].WhiteTools > 0)
                                    movesHandler.Add(new Moves(source, Constants.OutOfBoard, PlayerColor.White,pipe-(Constants.FieldLenght-source)));
                            }

                        }
                    }

                    foreach (var pipe in _diceState.PossibleLenghtMoves)
                    {

                        for (int i = 0; i < 24; i++)
                        {
                            if (_fields[i].IsAccessebleBy(color) && i - pipe >= 0)
                            {
                                int source = i - pipe;
                                if (_fields[source].WhiteTools > 0)
                                {
                                    movesHandler.Add(new Moves(source, i, PlayerColor.White));
                                }
                            }
                        }
                    }
                }
                else 
                {
                    foreach (var pipe in _diceState.PossibleLenghtMoves)
                    {
                        var target = pipe-1;
                        if (_fields[target].IsAccessebleBy(color))
                            movesHandler.Add(new Moves(Constants.BandWhite, target, PlayerColor.White ));
                    }
                }
            }
            else
            {
                if (!IsBlackStonesBanded())
                {
                    if (IsBlackInHome()) 
                    {
                        foreach (var pipe in _diceState.PossibleLenghtMoves)
                        {
                            int source = pipe;
                            for (int j = 0; j < source; j++)
                            {
                                if (j >= 0 && _fields[j].BlackTools > 0)
                                {
                                    movesHandler.Add(new Moves(j, Constants.OutOfBoard, PlayerColor.Black, pipe ));
                                }
                                  
                            }
                        }
                    }
                    foreach (var pipe in _diceState.PossibleLenghtMoves)
                    {
                        for (int i = 0; i < Constants.FieldLenght; i++)
                        {
                            if (_fields[i].IsAccessebleBy(color) && i + pipe < 24)
                            {
                                if (_fields[i + pipe].BlackTools > 0)
                                    movesHandler.Add(new Moves(i + pipe, i, PlayerColor.Black));
                            }
                        }
                    }
                } 
                else
                {
                    foreach (var pipe in _diceState.PossibleLenghtMoves)
                    {
                        var target = Constants.FieldLenght - pipe;
                        if (_fields[target].IsAccessebleBy(color))
                            movesHandler.Add(new Moves(Constants.BandBlack, target, PlayerColor.Black));
                    }
                }
            }
            return movesHandler.ToArray<Moves>();
        }

        public void SearchPossibleMoves()
        {
            if (_diceState == null || _currentStateType == StateType.PreGame) 
            {
                _possibleTargets = new Dictionary<int, int[]>();
                _possibleMoves = new Moves[0];
                _possibleSources = new int[0];
                return;
            }

            _possibleMoves = SearchAllPossibleMoves(_turn).ToArray(); 
            SeperateNewMovesToSorcesAndTargets();
        }

        private void SeperateNewMovesToSorcesAndTargets()
        {
            var dicMoves = new Dictionary<int, List<int>>();
            foreach (var move in _possibleMoves)
            {
                if (!dicMoves.ContainsKey(move.Source))
                    dicMoves[move.Source] = new List<int>();
                dicMoves[move.Source].Add(move.Target);
            }

            _possibleSources = dicMoves.Keys.ToArray();
            _possibleTargets = new Dictionary<int, int[]>();

            foreach (var move in dicMoves)
                _possibleTargets.Add(move.Key, move.Value.ToArray());
        }

        public TurnType CurrentTurn
        {
            get
            {
                if ( _diceState == null || CurrentSaType == StateType.PreGame) return TurnType.Roll;
                return TurnType.Move;
            }
        }

        public int MovesToEndFor(PlayerColor color) 
        {
            int sum = 0;
            sum = CalculateMovesToEndFor(color);
            return sum;
        }

        private int CalculateMovesToEndFor(PlayerColor playerColor)
        {
            int amount = 0;
            
            if(playerColor == PlayerColor.White)
                if (_fields[Constants.BandWhite].WhiteTools > 0) return -1; 
           
            if(playerColor== PlayerColor.Black)
                if (_fields[Constants.BandBlack].BlackTools > 0) return -1;
            
            for (int i = 0; i < Constants.FieldLenght; i++)
            {
                if(playerColor == PlayerColor.White)
                    amount += _fields[i].WhiteTools * (Constants.FieldLenght - i);
                else
                    amount += _fields[i].BlackTools * (1 + i);
            }
            return amount;
        }

        public static FieldBase[] InilizationFields()
        {
            var tempFields = new FieldBase[Constants.NumberOfFields];

            tempFields[Constants.BandWhite] = new FieldBand(PlayerColor.White,Constants.BandWhite);
            tempFields[Constants.BandBlack] = new FieldBand(PlayerColor.Black,Constants.BandBlack);
            tempFields[Constants.OutOfBoard] = new FieldOut(Constants.OutOfBoard);

            for (int i = 0; i < Constants.FieldLenght; ++i)
                tempFields[i] = new FieldBase(i);

            tempFields[0].WhiteTools = 2;
            tempFields[5].BlackTools = 5;
            tempFields[7].BlackTools = 3;
            tempFields[11].WhiteTools = 5;
            tempFields[12].BlackTools = 5;
            tempFields[16].WhiteTools = 3;
            tempFields[18].WhiteTools = 5;
            tempFields[23].BlackTools = 2;

            return tempFields;
        }
    }
}
