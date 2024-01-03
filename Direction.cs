using System;
using System.Collections.Generic;

namespace SnakeGame
{
    class Direction
    {
        // readonly variable: value can only be assigned once initially (constructor or variable definition)
        public readonly static Direction left = new Direction(0, -1);
        public readonly static Direction right = new Direction(0, 1);
        public readonly static Direction up = new Direction(-1, 0);
        public readonly static Direction down = new Direction(1, 0);
        
       
        //number of rows and columns moved per frame
        private int rowOffset;
        private int colOffset;

        public int RowOffset {  get { return rowOffset; } }
        public int ColOffset { get { return colOffset; } }

        //constructor is private as it is only used inside this class
        private Direction(int rowOffset, int colOffset)
        {
            this.rowOffset = rowOffset;
            this.colOffset = colOffset;
        }

        public Direction Opposite()
        {
            return new Direction(-RowOffset, -ColOffset);
        }


        //for testing
        public override string ToString()
        {
            return "(" + rowOffset + "," + colOffset + ")";
        }
        //******
        public override bool Equals(object obj)
        {
            //the following code is equivalent to "obj is Direction direction"
            //if(obj is Direction) 
            //{
            //    Direction direction = obj as Direction;
            //}
            return obj is Direction direction &&
                   RowOffset == direction.RowOffset &&
                   ColOffset == direction.ColOffset;

        }
        // explanation of Equals() method if it's not overridden
        /* Direction dir1 = new Direction(2, 0);
           Direction dir2 = new Direction(2,0);
           dir1.Equals(dir2) -> false

          Direction dir1 = new Direction(2, 0);
          Direction dir2 = dir1;
          dir1.Equals(dir2) -> true
         */

        //compiler gives warning if we override Equals() and GetHashCode() is not overriden
        public override int GetHashCode()
        {
            return HashCode.Combine(rowOffset, colOffset);
        }


        public static bool operator ==(Direction left, Direction right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }
    }
}
