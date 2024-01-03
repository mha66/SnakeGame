using System;

namespace SnakeGame
{
    class GameState
    {
        private int rows;
        private int columns;
        private GridValue[,] grid;
        private Direction currentDir;
        private int score;
        private bool gameOver;
        
        public int Rows {  get { return rows; }}
        public int Columns { get { return columns; }}
        public GridValue[,] Grid { get { return grid; }}
        public Direction CurrentDir { get { return currentDir; } private set { currentDir = value; } }
        public int Score { get { return score; } private set { score = value; } }
        public bool GameOver { get { return gameOver; } private set { gameOver = value; } }

        //linked list
        public SimpleLinkedList<Direction> dirChanges = new SimpleLinkedList<Direction>();
        public SimpleLinkedList<Position> snakePositions = new SimpleLinkedList<Position>();
        private readonly Random random = new Random();

        public GameState(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            grid = new GridValue[rows, columns];
            currentDir = Direction.right;
            score = 0;
            AddSnake();
            AddFood();
        }

        
        private void AddSnake()
        {
            int r = random.Next(rows);
           
            //example: if columns = 15 -> generate a random number n->   0 =< n < 15-4
            int randomCol = random.Next(0, columns - 4);
            for (int c = randomCol; c < randomCol + 3; c++) 
            {
                grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }
        }

        private SimpleLinkedList<Position> EmptyPositions()
        {
            SimpleLinkedList<Position> list = new SimpleLinkedList<Position>();
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    if (grid[r, c] == GridValue.Empty)
                        list.AddLast(new Position(r, c));
            return list;
        }

        private void AddFood()
        {
            //linked list
            SimpleLinkedList<Position> empty = EmptyPositions();
            //near impossible to happen
            if(empty.Size == 0)
                return;
            //random.Next(empty.Size) -> returns random index from 0 till Size-1
            Position pos = empty.Get(random.Next(empty.Size));
            grid[pos.Row, pos.Column] = GridValue.Food;
        }

        public Position HeadPosition()
        {
            return snakePositions.First;
        }

        public Position TailPosition()
        {
            return snakePositions.Last;
        }

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            grid[pos.Row, pos.Column] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last;
            grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection() 
        {
            if (dirChanges.Size == 0)
                return currentDir;
            return dirChanges.Last;
        }

        public bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Size == 4)
                return false;
            Direction lastDir =  GetLastDirection();

            return newDir != lastDir;
        }

        public void ChangeDirection(Direction dir)
        {
            if(CanChangeDirection(dir))
                dirChanges.AddLast(dir);
        }
         
        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= rows || pos.Column < 0 || pos.Column >= columns;
        }

        private GridValue WillHit (Position newHeadPos)
        {
            if(OutsideGrid(newHeadPos)) 
            {
                return GridValue.Outside;
            }

            //returns food, snake, or empty
            return grid[newHeadPos.Row, newHeadPos.Column];
        }

        public void Move()
        {
            if(dirChanges.Size >0)
            {
                currentDir = dirChanges.First;
                //Remove first
                dirChanges.Remove(dirChanges.First);
            }
            Position newHeadPos = HeadPosition().Translate(currentDir);
            GridValue hit = WillHit(newHeadPos);
            if(MainWindow.reverse)
            {
                snakePositions.Reverse();
                MainWindow.reverse = false;
            }

            else if (hit == GridValue.Outside) 
            {
               gameOver = true;
            }

            else if( hit == GridValue.Snake)
            {
                //if the snake is going to eat its "neck" undo the reverse
                if (newHeadPos == snakePositions.Head.Next.Data)
                {
                    snakePositions.Reverse();
                    dirChanges.AddLast(currentDir.Opposite());
                }
                else
                {
                    gameOver = true;
                }
            }

            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if(hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                score++;
                AddFood();
            }
        }
    }
}
