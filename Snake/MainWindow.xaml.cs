using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Media;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Threading;
using System.Threading;


namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum GamePhaseEnum { Start, Play, End, Stop }
    public partial class MainWindow : Window
    {
        private ImageSource tileSnake = new BitmapImage(new Uri("Assets/TileSnake.png", UriKind.Relative));
        private ImageSource tileApple = new BitmapImage(new Uri("Assets/TileApple.png", UriKind.Relative));
        private ImageSource tileEmpty = new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative));
        private ImageSource tileGoldenApple = new BitmapImage(new Uri("Assets/TileGoldenApple.png", UriKind.Relative));

        Image[,] tileImages;


        private GamePhaseEnum gamePhase;
        GameGrid gameGrid;
        int rows = 20;
        int cols = 20;

        DispatcherTimer timer = new DispatcherTimer();
        int timerValue;


        public MainWindow()
        {
            InitializeComponent();
            gamePhase = GamePhaseEnum.Start;

            //timerValue = 1200;
            timerValue = 5000;

            timer.Interval = TimeSpan.FromMilliseconds(timerValue);
            timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            gameGrid.MoveSnakeInTheLastDirection();
            Play();
        }

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Rewind the audio and play again
            BackgroundMusic.LoadedBehavior = MediaState.Manual;
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        private void NextGamePhase()
        {
            if (gamePhase == GamePhaseEnum.Start)
            {
                EndGameGrid.Visibility = Visibility.Hidden;

                StartGameGrid.Visibility = Visibility.Hidden;
                PlayGameGrid.Visibility = Visibility.Visible;
                gameGrid = new GameGrid(rows, cols);
                InitCanvas();
                gamePhase = GamePhaseEnum.Play;
                NextGamePhase();
            }
            else if (gamePhase == GamePhaseEnum.Play)
            {
                timer.Start();
                Play();
            }
            else if (gamePhase == GamePhaseEnum.End)
            {
                timer.Stop();
                PlayGameGrid.Visibility = Visibility.Hidden;
                EndGameGrid.Visibility = Visibility.Visible;
                
                if (gameGrid.GetTotalApplesEaten() + 1 >= rows * cols) //all tiles filled, snake cannot grow anymore
                {
                    GameOverTxtBlock.Text = "You freak. Go touch some grass.";
                    StartAgainButton.Content = "No.";
                }
                else
                {
                    GameOverTxtBlock.Text = "Game Over";
                    StartAgainButton.Content = "Play Again";
                }
            }
        }

        private void StartGameClick(object sender, RoutedEventArgs e)
        {
            gamePhase = GamePhaseEnum.Start;
            NextGamePhase();
        }

        private void Play()
        {
            bool isSnakeAlive = gameGrid.GetSnakeIsAlike();
            Draw();

            if (!isSnakeAlive)
            {
                gamePhase = GamePhaseEnum.End;
                NextGamePhase();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gamePhase == GamePhaseEnum.Play)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        gameGrid.MoveSnakeLeft();
                        Play();
                        break;

                    case Key.A:
                        gameGrid.MoveSnakeLeft();
                        Play();
                        break;

                    case Key.Right:
                        gameGrid.MoveSnakeRight();
                        Play();
                        break;

                    case Key.D:
                        gameGrid.MoveSnakeRight();
                        Play();
                        break;

                    case Key.Down:
                        gameGrid.MoveSnakeDown();
                        Play();
                        break;

                    case Key.S: 
                        gameGrid.MoveSnakeDown(); 
                        Play();
                        break;

                    case Key.Up:
                        gameGrid.MoveSnakeUp();
                        Play();
                        break;

                    case Key.W:
                        gameGrid.MoveSnakeUp();
                        Play();
                        break;

                    default:
                        break;
                }
            }
        }

        private void InitCanvas()
        {
            timerValue = 1200;
            timer.Interval = TimeSpan.FromMilliseconds(timerValue);

            ApplesTxtBlock.Text = "Apples: " + gameGrid.GetTotalApplesEaten().ToString();

            GameCanvas.Children.Clear();
            tileImages = new Image[rows, cols];

            double cellWidth = GameCanvas.Width / cols;
            double cellHeight = GameCanvas.Height / rows;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var img = new Image
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Source = null 
                    };
                    Canvas.SetLeft(img, col * cellWidth);
                    Canvas.SetTop(img, row * cellHeight);
                    GameCanvas.Children.Add(img);
                    tileImages[row, col] = img;
                }
            }
        }

        private void Draw()
        {
            double tileWidth = GameCanvas.Width / cols;
            double tileHeight = GameCanvas.Height / rows;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    tileImages[row, col].Source = tileEmpty; 
                }
            }

            foreach (var (x, y) in gameGrid.GetSnakeBody())
            {
                Image cellImage = tileImages[y, x];
                cellImage.Source = tileSnake;
                cellImage.Width = tileWidth;
                cellImage.Height = tileHeight;
            }

            var (appleX, appleY) = gameGrid.GetApplePosition();
            Image appleImage = tileImages[appleY, appleX];
            appleImage.Source = gameGrid.GetIsAppleGolden() ? tileGoldenApple : tileApple;
            appleImage.Width = tileWidth;
            appleImage.Height = tileHeight;

            ApplesTxtBlock.Text = "Apples: " + gameGrid.GetTotalApplesEaten().ToString();

        }


    }
}