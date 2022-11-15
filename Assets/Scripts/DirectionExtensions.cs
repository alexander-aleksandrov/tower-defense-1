using UnityEngine;

public static class DirectionExtensions
{
    private static Quaternion[] _rotations =
        { Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f),
        };
    public static Quaternion GetRotation(this Direction direction)
    {
        return _rotations[(int)direction];
    }

    public static DirectionChange GetDirectionChangeTo(this Direction current, Direction next)
    {
        if (current == next)
        {
            return DirectionChange.None;
        }

        if (current - 1 == next || current - 3 == next)
        {
            return DirectionChange.TurnRight;
        }

        if (current - 1 == next || current + 3 == next)
        {
            return DirectionChange.TurnLeft;
        }

        return DirectionChange.TurAround;
    }

    public static float GetAngle(this Direction direction)
    {
        return (float)direction * 90f;
    }

}

public enum Direction
{
    North, East, South, West
}

public enum DirectionChange
{
    None, TurnRight, TurnLeft, TurAround
}