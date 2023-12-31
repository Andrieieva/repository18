﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

class SnakeGame
{
    private int screenwidth;
    private int screenheight;
    private Snake snake;
    private List<Position> foods;
    private Direction direction;
    private int score;
    private Random random = new Random();

    public SnakeGame(int screenwidth, int screenheight)
    {
        this.screenwidth = screenwidth;
        this.screenheight = screenheight;
        snake = new Snake(screenwidth / 2, screenheight / 2);
        foods = new List<Position>();
        SpawnFood(3);
        direction = Direction.Right;
        score = 0;
    }

    public void Run()
    {
        while (true)
        {
            if (Console.KeyAvailable)
                direction = new InputOutput().GetNewDirection();

            Position newHead = snake.CalculateNewHeadPosition(direction);

            if (IsGameOver(newHead))
            {
                GameOver();
                break;
            }

            snake.Move(newHead);

            if (snake.EatFood(foods))
            {
                SpawnFood(1); 
                score++;
            }

            if (snake.Body.Count > 1)
            {
                Position tail = snake.Body.First();
                snake.Body.RemoveAt(0);
                Console.SetCursorPosition(tail.X, tail.Y);
                Console.Write(" ");
            }

            Draw();
            Thread.Sleep(100);
        }
    }

    private void GameOver()
    {
        Console.Clear();
        Console.SetCursorPosition(10, 10);
        Console.WriteLine("Game over! Your score: " + score);
        Console.SetCursorPosition(10, 11);
        Console.Write("Play again? (Y/N): ");
        char playAgain = Console.ReadKey(true).KeyChar;
        if (playAgain == 'Y' || playAgain == 'y')
        {
            Console.Clear();
            var game = new SnakeGame(screenwidth, screenheight);
            game.Run();
        }
    }

    private bool IsGameOver(Position newHead)
    {
        return newHead.X == 0 || newHead.X >= screenwidth - 1 || newHead.Y == 0 || newHead.Y >= screenheight - 1 || snake.IsCollisionWithSelf(newHead);
    }
    private void AddInitialFood(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int x = random.Next(1, screenwidth - 1);
            int y = random.Next(1, screenheight - 1);
            foods.Add(new Position(x, y));
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

    public void DrawSnake(Snake snake)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var segment in snake.Body)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("@");
        }
    }

    private void Draw()
    {
        Console.Clear();
        DrawRedBricks(screenwidth, screenheight);
        DrawFood(foods);
        DrawSnake(snake);
    }

    private void SpawnFood(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = random.Next(1, screenwidth - 1);
                y = random.Next(1, screenheight - 1);
            } while (snake.Body.Any(segment => segment.X == x && segment.Y == y) || foods.Any(food => food.X == x && food.Y == y));

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

