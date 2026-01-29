using UnityEngine;

using FrankenToilet.Core;

namespace FrankenToilet.greycsont;


public static class DirectionRandomizer
{
    private static int randomDirection;
    
    public static void GenerateRandomDirection() => randomDirection = Random.Range(0, 4);
    
    public static Vector3 Randomize4Dir(Vector3 direction)
    {
        float originalMag = direction.magnitude;

        Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;
        
        Vector3 resultDir;

        switch ((Direction)randomDirection)
        {
            case Direction.Upwards:
                resultDir = Quaternion.AngleAxis(-90, right) * direction;
                break;
            case Direction.Backwards:
                resultDir = -direction;
                break;
            case Direction.Right:
                resultDir = Quaternion.AngleAxis(90, Vector3.up) * direction;
                resultDir.y = -resultDir.y;
                break;
            case Direction.Left:
                resultDir = Quaternion.AngleAxis(-90, Vector3.up) * direction;
                resultDir.y = -resultDir.y;
                break;
            default:
                resultDir = direction;
                LogHelper.LogDebug("FUCK IENUMERATOR");
                break;
        }

        LogHelper.LogDebug($"Direction: {(Direction)randomDirection}");
        
        return resultDir.normalized * originalMag;;
    }
}


public enum Direction
{
    Backwards = 0,
    Upwards = 1,
    Left = 2,
    Right = 3
}