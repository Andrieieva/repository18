using System;
using System.Collections.Generic;
public class Snake
{
    private List<Position> body;
    private int growthSpurts;

    public Snake(Position initialPosition)
    {
        body = new List<Position> { initialPosition };
        growthSpurts = 0;
    }

    public IReadOnlyList<Position> Body => body;

    public int Length => body.Count;

    public Position Head => body.Last();

    public void Move(Direction direction)
    {
        Position newHead = CalculateNewHeadPosition(direction);
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
