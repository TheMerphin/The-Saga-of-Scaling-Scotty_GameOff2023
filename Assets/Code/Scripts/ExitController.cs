using UnityEngine;

public class ExitController : MonoBehaviour
{
    public StairDirection Direction;
}

public enum StairDirection
{
    Left = -1,
    Right = 1
}
