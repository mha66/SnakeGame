using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new Dictionary<GridValue, ImageSource>()
            {
                {GridValue.Empty,Images.Empty },
                {GridValue.Snake,Images.Body },
                {GridValue.Food,Images.Food },
            };

        private readonly Dictionary<Direction, int> dirToRotation = new Dictionary<Direction, int>()
            {
                {Direction.up, 0},
                {Direction.right, 90},
                {Direction.down, 180},
                {Direction.left, 270},
            };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning = false;
        public static bool reverse = false;
      
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);

        }

   
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image();
                    image.Source = Images.Empty;
                    //sets the rotation point of the image to its center
                    image.RenderTransformOrigin = new Point(0.5, 0.5);
           
                    //Image Source
                    images[r, c] = image;
                    //display the images inside GameGrid
                    GameGrid.Children.Add(image);

                }
            }
            return images;
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            OverLay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();

            //restarts game positions
            gameState = new GameState(rows, cols);

        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                }
            }
        }

        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Column];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.CurrentDir];
            image.RenderTransform = new RotateTransform(rotation);

        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }


        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
                //ErrorBox.Text = "Current direction:" + gameState.CurrentDir.ToString() + "\nBuffer:" + gameState.dirChanges.ToString() + "\nreversed:" + reverse;
            }

        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            OverLay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }


        private async Task DrawDeadSnake()
        {

            for (int i = 0; i < gameState.snakePositions.Size; i++)
            {
                Position pos = gameState.snakePositions.Get(i);
                ImageSource source;
                if (i == 0)
                    source = Images.DeadHead;
                else
                    source = Images.DeadBody;
                gridImages[pos.Row, pos.Column].Source = source;
                await Task.Delay(50);
            }

        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (OverLay.Visibility == Visibility.Visible)
                e.Handled = true;
            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (gameState.GameOver)
                return;
            if (reverse == false)
            {
                switch (e.Key)
                {
                    case Key.A:


                        if (gameState.CurrentDir == Direction.right)
                            reverse = true;
                        gameState.ChangeDirection(Direction.left);

                        break;
                    case Key.D:

                        if (gameState.CurrentDir == Direction.left)
                            reverse = true;
                        gameState.ChangeDirection(Direction.right);

                        break;
                    case Key.W:


                        if (gameState.CurrentDir == Direction.down)
                            reverse = true;
                        gameState.ChangeDirection(Direction.up);

                        break;
                    case Key.S:

                        if (gameState.CurrentDir == Direction.up)
                            reverse = true;
                        gameState.ChangeDirection(Direction.down);

                        break;
                    
                }
                
            }
           
        }
    }
}

