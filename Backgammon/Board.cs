using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class Board
    {
        private FieldBase[] _fields;
        private int[] _possibleSources;
        private Dictionary<int, int[]> _possibleTargets;
        private Dice _dice;

        public FieldBase[] Fileds => _fields;
        public int[] PossibleSources => _possibleSources;
        public Dictionary<int, int[]> PssibleTargets => _possibleTargets;

        public Board(FieldBase[] fields,Dice dice, int[] possibleSources, Dictionary<int, int[]> possibleTargets)
        {
            _fields = new FieldBase[26];
            //_fields =;
            //    fields.CopyTo(_fields, 26);
            for (int i =0 ; i<26 ; i++)
            {
                _fields[i] = fields[i];
            }
            _possibleSources = possibleSources;
            _possibleTargets = possibleTargets;
            _dice = dice;
        }


    }
}
