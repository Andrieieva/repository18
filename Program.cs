using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WindowHeight = 20;
        Console.WindowWidth = 44;
        int screenwidth = Console.WindowWidth;
        int screenheight = Console.WindowHeight;
        Random random = new Random();

        Queue<Position> snake = new Queue<Position>();
        snake.Enqueue(new Position(screenwidth / 2, screenheight / 2));
        int snakeLength = 1;
        int score = 0;
        bool gameOver = false;

        List<Position> foods = new List<Position>();
        SpawnFood(foods, random, screenwidth, screenheight, 3);

        Direction direction = Direction.Right;

        DrawRedBricks(screenwidth, screenheight);

        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
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

            Position head = snake.Last();
            Position newHead = CalculateNewHeadPosition(head, direction);

            if (newHead.X == 0 || newHead.X >= screenwidth - 1 || newHead.Y == 0 || newHead.Y >= screenheight - 1 || snake.Contains(newHead))
            {
                gameOver = true;
                break;
            }

            snake.Enqueue(newHead);
            Console.SetCursorPosition(newHead.X, newHead.Y);
            Console.Write("@");

            bool foodEaten = false;
            foreach (var food in foods)
            {
                if (newHead.Equals(food))
                {
                    score++;
                    snakeLength++;
                    foodEaten = true;
                    foods.Remove(food);
                    break;
                }
            }

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

            foreach (var food in foods)
            {
                Console.SetCursorPosition(food.X, food.Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("o");
            }

            Thread.Sleep(200);
        }

        GameOver(score);
        Console.SetCursorPosition(10, 12);
        Console.Write("Play again? (Y/N): ");
        char playAgain = Console.ReadKey(true).KeyChar;
        if (playAgain == 'Y' || playAgain == 'y')
        {
            Console.Clear();
            Main(args);
        }
    }

    static Position CalculateNewHeadPosition(Position head, Direction direction)
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

    static void GameOver(int score)
    {
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("Game over! Your score: " + score);
    }

    static void SpawnFood(List<Position> foods, Random random, int screenwidth, int screenheight, int count)
    {
        foods.Clear();
        for (int i = 0; i < count; i++)
        {
            int x = random.Next(1, screenwidth - 1);
            int y = random.Next(1, screenheight - 1);
            foods.Add(new Position(x, y));
        }
    }

    static void DrawRedBricks(int screenWidth, int screenHeight)
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
