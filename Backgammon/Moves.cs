using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backgammon
{
    public class Moves : IComparable<Moves>, IEquatable<Moves>
    {
        private int _length;

        public int Source { get; }
        public int Target { get; }
        public PlayerColor Color { get; }
        public int Lenght => _length;

        public bool IsEmpty => Source == 0 && Target == 0;

        public Moves(int source, int target, PlayerColor color, int addLength=0)
        {
            Source = source;
            Target = target;
            Color = color;

            if (source == Constants.BandBlack)
                _length = Constants.FieldLenght - target;
            else if (source == Constants.BandWhite)
                _length = Target + 1 ;
            else if (target == Constants.OutOfBoard)
            {
                if (color == PlayerColor.White)
                    _length = Constants.FieldLenght - source + addLength;
                else
                    _length = addLength;
            }
            else
            {
                _length = Math.Abs(target - source);
            }
        }
        
        public int CompareTo(Moves other)
        {
            if (Source > other.Source)
                return 1;
            if (Source < other.Source)
                return -1;
            if (Target > other.Target)
                return 1;
            if (Target < other.Target)
                return -1;
            if (Color > other.Color)
                return 1;
            if (Color < other.Color)
                return -1;
            return 0;
        }

        public bool Equals(Moves other)
        {
            return (Color == other.Color && Target == other.Target && Source == other.Source);
        }

        public override int GetHashCode()
        {
            int r = 100 * Source + Target;
            if (Color == PlayerColor.White) r += 30000;
            else r += 50000;
            return r;
        }

        public static Moves EmptyMove(PlayerColor color)
        {
            return new Moves(0, 0, color);
        }
    }
}
