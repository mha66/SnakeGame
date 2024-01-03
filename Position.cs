using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Position
    {
        private int row;
        private int column;

        public int Row {  get { return row; } }
        public int Column { get { return column; } }

        public Position(int row, int column) 
        {
            this.row = row;
            this.column = column;
        }

        //moves position in given direction 
        public Position Translate(Direction dir)
        {
            return new Position(row + dir.RowOffset, column + dir.ColOffset);
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}
