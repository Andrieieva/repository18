using System;

class InputOutput
{
    public Direction GetNewDirection()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        return Direction.Up;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        return Direction.Down;
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        return Direction.Left;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        return Direction.Right;
                }
            }
        }
    }
}


