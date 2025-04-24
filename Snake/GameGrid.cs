using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    enum LastSnakeMoveEnum { Right, Left, Up, Down };
    internal class GameGrid
    {
        Random random = new Random();
        LastSnakeMoveEnum _lastSnakeMove;

        int _rows;
        int _columns;

        Queue<(int x, int y)> _snake;
        bool _snakeAlive;

        int _growthPending;
        bool _isAppleGolden;
        int _appleX;
        int _appleY;

        int _totalApplesEaten;

        public GameGrid(int rows, int columns) 
        {
            _rows = rows;
            _columns = columns;

            _snake = new Queue<(int x, int y)>();
            _snake.Enqueue(( 0, 0 ));

            _growthPending = 0;
            _snakeAlive = true;

            SpawnNewApple();
            _totalApplesEaten = 0;
            _isAppleGolden = false;


            _lastSnakeMove = LastSnakeMoveEnum.Down; //snake starts moving down
        }

        public void MoveSnakeInTheLastDirection()
        {
            if(_lastSnakeMove == LastSnakeMoveEnum.Right)
            {
                MoveSnakeRight();
            }
            else if (_lastSnakeMove == LastSnakeMoveEnum.Left)
            {
                MoveSnakeLeft();
            }
            else if (_lastSnakeMove == LastSnakeMoveEnum.Up)
            {
                MoveSnakeUp();
            }
            else if (_lastSnakeMove == LastSnakeMoveEnum.Down)
            {
                MoveSnakeDown();
            }
        }

        public bool GetSnakeIsAlike()
        {
            return _snakeAlive;
        }

        public IEnumerable<(int x, int y)> GetSnakeBody()
        {
            return _snake;
        }

        public (int x, int y) GetApplePosition()
        {
            return (_appleX, _appleY);
        }

        public int GetTotalApplesEaten()
        {
            return _totalApplesEaten;
        }

        public bool GetIsAppleGolden()
        {
            return _isAppleGolden;
        }

        public void MoveSnakeLeft()
        {
            if (_lastSnakeMove == LastSnakeMoveEnum.Right)
            {
                return; //player is trying to move the snake back to its current possition
            }

            var head = _snake.Last();
            if (_snake.Last().x == 0) //check for wall
            {
                _snakeAlive = false;
                return;
            }
            CheckIfTheSnakeIsEatingApple(head.x - 1, head.y);
            if (_snake.Contains((head.x - 1, head.y)))
            {
                _snakeAlive = false;
                return;
            }
            _snake.Enqueue((_snake.Last().x - 1, _snake.Last().y));


            if (_growthPending > 0)
            {
                _growthPending--; //do not remove tail, snake is growing
            }
            else
            {
                _snake.Dequeue(); //remove tail
            }

            _lastSnakeMove = LastSnakeMoveEnum.Left;
        }

        public void MoveSnakeRight()
        {
            if (_lastSnakeMove == LastSnakeMoveEnum.Left)
            {
                return; //player is trying to move the snake back to its current possition
            }

            var head = _snake.Last();
            if (_snake.Last().x == _columns - 1) //check for wall
            {
                _snakeAlive = false;
                return;
            }
            CheckIfTheSnakeIsEatingApple(head.x + 1, head.y);
            if (_snake.Contains((head.x + 1, head.y)))
            {
                _snakeAlive = false;
                return;
            }
            _snake.Enqueue((_snake.Last().x + 1, _snake.Last().y));


            if (_growthPending > 0)
            {
                _growthPending--; //do not remove tail, snake is growing
            }
            else
            {
                _snake.Dequeue(); //remove tail
            }

            _lastSnakeMove = LastSnakeMoveEnum.Right;
        }

        public void MoveSnakeUp()
        {
            if (_lastSnakeMove == LastSnakeMoveEnum.Down)
            {
                return; //player is trying to move the snake back to its current possition
            }

            var head = _snake.Last();
            if (_snake.Last().y == 0) //check for wall
            {
                _snakeAlive = false;
                return;
            }
            CheckIfTheSnakeIsEatingApple(head.x, head.y - 1);
            if (_snake.Contains((head.x, head.y - 1)))
            {
                _snakeAlive = false;
                return;
            }
            _snake.Enqueue((_snake.Last().x, _snake.Last().y - 1));


            if (_growthPending > 0)
            {
                _growthPending--; //do not remove tail, snake is growing
            }
            else
            {
                _snake.Dequeue(); //remove tail
            }

            _lastSnakeMove = LastSnakeMoveEnum.Up;
        }

        public void MoveSnakeDown()
        {
            if (_lastSnakeMove == LastSnakeMoveEnum.Up)
            {
                return; //player is trying to move the snake back to its current possition
            }

            var head = _snake.Last();
            if (_snake.Last().y == _rows - 1) //check for wall
            {
                _snakeAlive = false;
                return;
            }
            CheckIfTheSnakeIsEatingApple(head.x, head.y + 1);
            if (_snake.Contains((head.x, head.y + 1)))
            {
                _snakeAlive = false;
                return;
            }
            _snake.Enqueue((_snake.Last().x, _snake.Last().y + 1));


            if (_growthPending > 0)
            {
                _growthPending--; //do not remove tail, snake is growing
            }
            else
            {
                _snake.Dequeue(); //remove tail
            }

            _lastSnakeMove = LastSnakeMoveEnum.Down;
        }

        public void SpawnNewApple()
        {
            _appleX = new Random().Next(1, _columns - 1);
            _appleY = new Random().Next(1, _rows - 1);


            int rnd = new Random().Next(6);
            if (rnd == 3)
            {
                _isAppleGolden = true;
            }
            else
            {
                _isAppleGolden = false;
            }
        }   

        public void CheckIfTheSnakeIsEatingApple(int headX, int headY)
        {
            if (headX == _appleX && headY == _appleY)
            {
                if (_isAppleGolden)
                {
                    _totalApplesEaten += 3;
                    _growthPending += 3;
                }
                else
                {
                    _totalApplesEaten++;
                    _growthPending += 1;
                }
                SpawnNewApple();
            }
        }

    }
}
