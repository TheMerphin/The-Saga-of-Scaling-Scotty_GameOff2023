using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Toolbox
{
    public static DiagonalDirection GetDiagonalDirection(Vector2 vector)
    {
        if (vector.x >= 0 && vector.y > 0)
            return DiagonalDirection.UpRight;
        else if (vector.x < 0 && vector.y >= 0)
            return DiagonalDirection.UpLeft;
        else if (vector.x <= 0 && vector.y < 0)
            return DiagonalDirection.DownLeft;
        else // if (vector.x > 0 && vector.y <= 0)
            return DiagonalDirection.DownRight;
    }

    public enum DiagonalDirection
    {
        UpRight,
        UpLeft,
        DownRight, // Default state
        DownLeft
    }
}
