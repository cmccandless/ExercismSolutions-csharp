using System;

public enum Direction
{
    North,
    East,
    South,
    West
}

public class RobotSimulator
{
    public RobotSimulator(Direction direction, int x, int y)
    {
        Direction = direction;
        X = x;
        Y = y;
    }

    public Direction Direction { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public void Move(string instructions)
    {
        foreach (var instruction in instructions.ToCharArray())
        {
            switch(instruction)
            {
                case 'R': Direction = (Direction)(((int)Direction + 1) % 4); break;
                case 'L': Direction = (Direction)(((int)Direction + 3) % 4); break;
                case 'A':
                    switch (Direction)
                    {
                        case Direction.North: Y++; break;
                        case Direction.South: Y--; break;
                        case Direction.East: X++; break;
                        case Direction.West: X--; break;
                    }
                    break;
            }
        }
    }
}
