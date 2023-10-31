using System;
using System.Collections.Generic;
using System.Linq;

class Snake
{
    private List<Position> body;
    private int growthSpurts;
    public Snake(int initialX, int initialY)
    {
        body = new List<Position> { new Position(initialX, initialY) };
        growthSpurts = 0;
        CurrentDirection = Direction.Right;
    }
    public IReadOnlyList<Position> Body => body;
    public int Length => body.Count;

    public Position Head => body.Last();
    public Direction CurrentDirection { get; private set; }

    public void ChangeDirection(Direction newDirection)
    {
        if (newDirection == Direction.Left && CurrentDirection != Direction.Right ||
            newDirection == Direction.Right && CurrentDirection != Direction.Left ||
            newDirection == Direction.Up && CurrentDirection != Direction.Down ||
            newDirection == Direction.Down && CurrentDirection != Direction.Up)
        {
            CurrentDirection = newDirection;
        }
    }
    public void Move()
    {
        Position newHead = CalculateNewHeadPosition(CurrentDirection);
        body.Add(newHead);

        if (growthSpurts > 0)
        {
            growthSpurts--;
        }
        else
        {
            body.RemoveAt(0);
        }
    }
    public void EatFood()
    {
        growthSpurts++;
    }
    public bool IsCollisionWithSelf()
    {
        for (int i = 0; i < body.Count - 1; i++)
        {
            if (body[i].Equals(Head))
            {
                return true;
            }
        }
        return false;
    }
    private Position CalculateNewHeadPosition(Direction direction)
    {
        Position head = Head;

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
}
