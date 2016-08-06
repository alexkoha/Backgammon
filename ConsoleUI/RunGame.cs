using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Backgammon;

namespace ConsoleUI
{
    public class RunGame : IGameControllEvents
    {
        private Controller _game;
        public Controller Game => _game;

        public RunGame()
        {
            NewGame();
        }

        public void NewGame() 
        {
            Console.WriteLine("Welcome to Backgammon game !\nLets start new game.\n\nChoose Game Mode :\n[1] Human Player vs Human Player\n[2] Human Player vs PC Player");
            bool isInputRight = false;
            while (!isInputRight)
            {
                int choose;
                var input = Console.ReadLine();
                isInputRight = int.TryParse(input, out choose) && choose == 1 || choose == 2;
                if (isInputRight)
                {
                    if (choose == 1)
                        _game = new Controller(new PlayerHuman(PlayerColor.White, PlayerType.Human),
                            new PlayerHuman(PlayerColor.Black, PlayerType.Human), this);
                    if (choose == 2)
                        _game = new Controller(new PlayerHuman(PlayerColor.White, PlayerType.Human),
                            new PlayerPc(PlayerColor.Black, PlayerType.PC),this);
                }
                else
                {
                    Console.WriteLine($"Error input.Attention !\nYou must choose number 1 or 2!");
                }

            }
            _game.Events.OnUpdateGame();
        }

        public void GameEnd(object sender, EventArgs e)
        {
            WinnerPanel();
            _game = null;
            Environment.Exit(0);
        }

        public void MakeMove(object sender, EventArgs e)
        {
            _game.Events.OnUpDateScreen();

            bool checkInput = false;
            int source = 0;
            int target = 0;

            Console.WriteLine("Make Move:");
            Console.WriteLine("----------");

            if (Game.GameState.IsBandedStones(Game.CurrentPlayer))
            {
                source = Game.CurrentPlayer.PlayerColor == PlayerColor.White ? Constants.BandWhite + 1 : Constants.BandBlack + 1;
                Console.WriteLine("You Have Banded Stones ! \n");
            }
            else
            {
                while (!checkInput)
                {
                    Console.WriteLine("Enter source :");
                    var sourceEntered = Console.ReadLine();
                    checkInput = int.TryParse(sourceEntered, out source) && source > 0 && source < 25;
                    if (!checkInput)
                    {
                        Console.WriteLine("Source must be 1-24.\nTry again.\n[Enter] continue");
                        Console.ReadLine();
                    }
                }
            }

            checkInput = false;
            while (!checkInput)
            {
                Console.WriteLine("Enter Target :");
                var targerEntered = Console.ReadLine();
                int.TryParse(targerEntered, out target);

                checkInput = int.TryParse(targerEntered, out target) && target > 0 && target < 25 || target == 26;
                if (!checkInput)
                {
                    Console.WriteLine("Target must be 1-24 or 27(out).\nTry again.\n[Enter] continue");
                    Console.ReadLine();
                }
            }

            try
            {
                var isMoveDone = Game.CurrentPlayer.MakeMove(source - 1, target - 1);
                if (isMoveDone == false)
                {
                    Console.WriteLine("This Move wrong.Try Again!");
                    Console.WriteLine("[Enter] Continue");
                    Console.ReadLine();
                }
            }
            catch (IndexOutOfRangeException exception)
            {
                Console.WriteLine(exception.Message);
                Console.Read();
            }

            

            _game.Events.OnUpDateScreen();
            _game.Events.OnUpdateGame();
        }

        public void NoneMoves(object sender, EventArgs e)
        {
            _game.Events.OnUpDateScreen();
            Console.WriteLine("You Have Not Some Ligal Moves.\nYour turn passed to other player !\n\nEnter] Continue");
            Console.Read();
        }

        public void RollDice(object sender, EventArgs e)
        {
            _game.Events.OnUpDateScreen();
            if (Game.CurrentPlayer.PlayerType == PlayerType.Human)
            {
                Console.WriteLine("[Enter] Roll Dices ! ");
                Console.ReadLine();
            }

            try
            {
                _game.RollTheDice(_game.CurrentPlayer);
            }
            catch(Exception exn)
            {
                Console.WriteLine(exn.Message);
            }

            var isPreGame = _game.GameState.CurrentSaType == StateType.PreGame;
            if (isPreGame)
            {
                Console.WriteLine($"Dices : [{_game.Dice.DiceOne}] [{_game.Dice.DiceTwo}]");
                Console.WriteLine("\n[Enter] Continue ");
            }
            else
            {
                Console.WriteLine($"Dices : [{_game.Dice.DiceOne}] [{_game.Dice.DiceTwo}]\n");
                Console.WriteLine("[Enter] Continue ");
            }
            Console.ReadLine();

            _game.Events.OnUpDateScreen();
            _game.Events.OnUpdateGame();
        }

        public void UpDateScreen(object sender, EventArgs e)
        {
            Console.Clear();
            PrintBoard();
            if (_game.GameState.CurrentSaType == StateType.Real)
            {
                PrintPanel();
            }
            else
            {
                 PreGamePanel();
            }
        }

        private void WinnerPanel()
        {
            Console.Clear();
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~~~~~~~~~~Congratulations!!!~~~~~~~~~");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("~~~~~~~~~~~~~~~~Winner~~~~~~~~~~~~~~~");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
            Console.WriteLine($"The {_game.CheckForWinner()} Player is the winner !!!!!!");
            Console.ReadLine();
        }

        private void PreGamePanel()
        {
            Console.WriteLine("------------------Pregame------------------------");
            Console.WriteLine($"Player : {_game.CurrentPlayer.PlayerColor }\n");
        }

        private void PrintPanel()
        {
            Console.WriteLine($"Black output: {_game.BlackOuts} | White output: {_game.WhiteOuts} | Out Move = 27 ");
            Console.WriteLine("------------------Panel--------------------------");
            Console.WriteLine($"|\tPlayer : {_game.GameState.Turn}\t Movies to End : {_game.GameState.MovesToEndFor(_game.GameState.Turn)} \t|");
            Console.WriteLine("-------------------------------------------------");
            if (_game.Dice == null)
            {
                Console.WriteLine($"|\tDices \t [0] [0]\t\t\t|");
                Console.WriteLine("-------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"|\tDices \t [{_game.Dice.DiceOne}] [{_game.Dice.DiceTwo}]\t\t\t|");
                Console.WriteLine("-------------------------------------------------");
            }

        }

        private void PrintBoard()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            FieldBase[] fields = null;
            fields = _game.GetBoard().Fileds;


            Console.WriteLine("|###############################################|");
            Console.Write("|");

            for (int i = 0; fields[Constants.BandBlack].BlackTools > i; i++)
            {
                PrintStoneBand(ConsoleColor.Black);
            }
            Console.WriteLine("\t\t\t\t\t\t|");
            Console.WriteLine("| 13| 14| 15| 16| 17| 18| 19| 20| 21| 22| 23| 24|");
            Console.WriteLine("|***|***|***|***|***|***|***|***|***|***|***|***|");

            PrintUpSideOfBoard(fields);
            PrintDownSideOfBoard(fields);

            Console.WriteLine("|***|***|***|***|***|***|***|***|***|***|***|***|");
            Console.WriteLine("| 12| 11| 10| 9 | 8 | 7 | 6 | 5 | 4 | 3 | 2 | 1 |");
            Console.Write("|");

            for (int i = 0 ; fields[Constants.BandWhite].WhiteTools > i ; i++)
            {
                PrintStoneBand(ConsoleColor.White);
            }
            Console.WriteLine("\t\t\t\t\t\t|");
            Console.WriteLine("|###############################################|");
            Console.ResetColor();
        }

        private static void PrintDownSideOfBoard(FieldBase[] fields)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"|");
                for (int j = 11; j >= 0; j--)
                {
                    if (fields[j].BlackTools > 9-i)
                    {
                        if (fields[j].BlackTools > 9)
                        {
                            Console.Write(i == 9 ? $" {fields[j].BlackTools}|" : $"   |");
                        }
                        else
                        {
                            PrintStone(ConsoleColor.Black);
                        }
                    }
                    else if (fields[j].WhiteTools > 9-i)
                    {
                        if (fields[j].WhiteTools > 9)
                        {
                            if (i == 9)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {fields[j].WhiteTools}");
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("|");
                            }
                            else
                                Console.Write($"   |");  
                        }
                        else
                        {
                            PrintStone(ConsoleColor.White);
                        }
                    }
                    else
                    {
                        Console.Write($"   |");
                    }

                }
                Console.WriteLine();
            }
        }

        private static void PrintUpSideOfBoard(FieldBase[] fields)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"|");
                for (int j = 12; j < 24; j++)
                {
                    if (fields[j].BlackTools > i)
                    {
                        if (fields[j].BlackTools > 9)
                        {
                            Console.Write(i == 0 ? $" {fields[j].BlackTools}|" : $"   |");
                        }
                        else
                        {
                            PrintStone(ConsoleColor.Black);
                        }
                    }
                    else if (fields[j].WhiteTools > i)
                    {
                        if (fields[j].WhiteTools > 9)
                        {
                            if (i == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($" {fields[j].WhiteTools}");
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("|");
                            }
                            else
                                Console.Write($"   |");
                        }
                        else
                        {
                            PrintStone(ConsoleColor.White);
                        }
                    }
                    else
                    {
                        Console.Write($"   |");
                    }
                }
                Console.WriteLine();
            }
        }

        private static void PrintStone(ConsoleColor color)
        {
            Console.Write(" ");
            Console.ForegroundColor = color;
            Console.Write("O");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" |");
        }

        private static void PrintStoneBand(ConsoleColor color)
        {

            Console.ForegroundColor = color;
            Console.Write("O");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }
    }
}
