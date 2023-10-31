using System;
using System.Collections.Generic;

class InputOutput
{
    public Direction GetNewDirection(ConsoleKeyInfo key, Direction currentDirection)
    {
        switch (key.Key)
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

    public void DrawSnake(Snake snake)
    {
        foreach (var segment in snake.Body)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("@");
        }
    }

    public void DrawFood(List<Position> foods)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var food in foods)
        {
            Console.SetCursorPosition(food.X, food.Y);
            Console.Write("o");
        }
    }

    public ConsoleKey GetUserInput()
    {
        if (Console.KeyAvailable)
        {
            return Console.ReadKey(true).Key;
        }
        return 0;
    }

    public bool DisplayGameOver(int score)
    {
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("Game over! Your score: " + score);
        Console.SetCursorPosition(10, 11);
        Console.Write("Play again? (Y/N): ");
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Y)
                return true;
            if (keyInfo.Key == ConsoleKey.N)
                return false;
        }
    }
}
