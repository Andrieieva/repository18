using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WindowHeight = 16;
        Console.WindowWidth = 32;
        int screenwidth = Console.WindowWidth;
        int screenheight = Console.WindowHeight;
        Random random = new Random();

        SnakeGame game = new SnakeGame(screenwidth, screenheight);

        game.Start();
    }
}

class SnakeGame
{
    private int screenwidth;
    private int screenheight;
    private Random random;
    private Queue<Position> snake;
    private int snakeLength;
    private int score;
    private bool gameOver;
    private List<Position> foods;
    private Direction direction;

    public SnakeGame(int width, int height)
    {
        screenwidth = width;
        screenheight = height;
        random = new Random();

        snake = new Queue<Position>();
        snake.Enqueue(new Position(screenwidth / 2, screenheight / 2));
        snakeLength = 1;
        score = 0;
        gameOver = false;

        foods = new List<Position>();
        SpawnFood(foods, random, screenwidth, screenheight, 3);

        direction = Direction.Right;
        DrawRedBricks(screenwidth, screenheight);
    }

    public void Start()
    {
        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                ChangeDirection(key);
            }

            Position head = snake.Last();
            Position newHead = CalculateNewHeadPosition(head, direction);

            if (IsCollision(newHead))
            {
                gameOver = true;
                break;
            }

            snake.Enqueue(newHead);
            Console.SetCursorPosition(newHead.X, newHead.Y);
            Console.Write("■");

            bool foodEaten = CheckFoodCollision(newHead);
            if (foodEaten)
            {
                SpawnFood(foods, random, screenwidth, screenheight, 2);
            }
            else
            {
                if (snake.Count > snakeLength)
                {
                    var tail = snake.Dequeue();
                    Console.SetCursorPosition(tail.X, tail.Y);
                    Console.Write(" ");
                }
            }

            DrawFoods();

            Thread.Sleep(150);
        }

        GameOver(score);
        Console.SetCursorPosition(10, 12);
        Console.Write("Play again? (Y/N): ");
        char playAgain = Console.ReadKey(true).KeyChar;
        if (playAgain == 'Y' || playAgain == 'y')
        {
            Console.Clear();
            Start();
        }
    }

    private void ChangeDirection(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
                if (direction != Direction.Down)
                    direction = Direction.Up;
                break;
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
                if (direction != Direction.Up)
                    direction = Direction.Down;
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                if (direction != Direction.Right)
                    direction = Direction.Left;
                break;
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                if (direction != Direction.Left)
                    direction = Direction.Right;
                break;
        }
    }

    private Position CalculateNewHeadPosition(Position head, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Position(head.X, head.Y - 1);
            case Direction.Down:
                return new Position(head.X, head.Y + 1);
            case Direction.Left:
                return new Position(head.X - 1, head.Y);
            case Direction.Right:
                return new Position(head.X + 1, head.Y);
            default:
                return head;
        }
    }

    private bool IsCollision(Position newHead)
    {
        return newHead.X == 0 || newHead.X >= screenwidth - 1 || newHead.Y == 0 || newHead.Y >= screenheight - 1 || snake.Contains(newHead);
    }

    private bool CheckFoodCollision(Position newHead)
    {
        foreach (var food in foods)
        {
            if (newHead.Equals(food))
            {
                score++;
                snakeLength++;
                foods.Remove(food);
                return true;
            }
        }
        return false;
    }

    private void DrawFoods()
    {
        foreach (var food in foods)
        {
            Console.SetCursorPosition(food.X, food.Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("■");
        }
    }

    private void GameOver(int score)
    {
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("Game over! Your score: " + score);
    }

    private void SpawnFood(List<Position> foods, Random random, int screenwidth, int screenheight, int count)
    {
        foods.Clear();
        for (int i = 0; i < count; i++)
        {
            int x = random.Next(1, screenwidth - 1);
            int y = random.Next(1, screenheight - 1);
            foods.Add(new Position(x, y));
        }
    }

    private void DrawRedBricks(int screenWidth, int screenHeight)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        for (int i = 0; i < screenWidth; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write("■");
            Console.SetCursorPosition(i, screenHeight - 1);
            Console.Write("■");
        }
        for (int i = 0; i < screenHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("■");
            Console.SetCursorPosition(screenWidth - 1, i);
            Console.Write("■");
        }
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

struct Position
{
    public int X { get; }
    public int Y { get; }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Position other)
    {
        return X == other.X && Y == other.Y;
    }
}
