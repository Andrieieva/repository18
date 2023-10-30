using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    static void Main(string[] args)
    {
        Console.WindowHeight = 22;
        Console.WindowWidth = 40;
        int screenwidth = Console.WindowWidth;
        int screenheight = Console.WindowHeight;
        Random random = new Random();

        Direction direction = Direction.Right;

        Snake snake = new Snake(new Position(screenwidth / 2, screenheight / 2));

        List<Position> foods = new List<Position>();
        SpawnFood(foods, random, screenwidth, screenheight, 3);

        DrawRedBricks(screenwidth, screenheight);

        bool gameOver = false;
        int score = 0;

        while (!gameOver)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                direction = GetNewDirection(key, direction);
            }

            snake.Move(direction);
            Position head = snake.Head;

            if (head.X == 0 || head.X >= screenwidth - 1 || head.Y == 0 || head.Y >= screenheight - 1 || snake.IsCollisionWithSelf())
            {
                gameOver = true;
                break;
            }

            bool foodEaten = false;
            foreach (var food in foods)
            {
                if (head.Equals(food))
                {
                    score++;
                    snake.EatFood();
                    foodEaten = true;
                    foods.Remove(food);
                    break;
                }
            }

            if (foodEaten)
            {
                SpawnFood(foods, random, screenwidth, screenheight, 1);
            }

            Console.Clear();
            DrawRedBricks(screenwidth, screenheight);
            foreach (var food in foods)
            {
                Console.SetCursorPosition(food.X, food.Y);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("o");
            }

            foreach (var segment in snake.Body)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write("@");
            }

            Thread.Sleep(150);
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

    static Direction GetNewDirection(ConsoleKey key, Direction currentDirection)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (currentDirection != Direction.Down)
                    return Direction.Up;
                break;
            case ConsoleKey.DownArrow:
                if (currentDirection != Direction.Up)
                    return Direction.Down;
                break;
            case ConsoleKey.LeftArrow:
                if (currentDirection != Direction.Right)
                    return Direction.Left;
                break;
            case ConsoleKey.RightArrow:
                if (currentDirection != Direction.Left)
                    return Direction.Right;
                break;
            case ConsoleKey.W:
                if (currentDirection != Direction.Down)
                    return Direction.Up;
                break;
            case ConsoleKey.S:
                if (currentDirection != Direction.Up)
                    return Direction.Down;
                break;
            case ConsoleKey.A:
                if (currentDirection != Direction.Right)
                    return Direction.Left;
                break;
            case ConsoleKey.D:
                if (currentDirection != Direction.Left)
                    return Direction.Right;
                break;
        }
        return currentDirection;
    }
    static void GameOver(int score)
    {
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("Game over! Your score: " + score);
    }

    static void SpawnFood(List<Position> foods, Random random, int screenwidth, int screenheight, int count)
    {
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

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public struct Position
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

