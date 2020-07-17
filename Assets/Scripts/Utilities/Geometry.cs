using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A collection of geometric helper functions.
/// </summary>
public class Geometry
{
    public static Vector2? LineIntersection((Vector2, Vector2) a, (Vector2, Vector2) b)
    {
        if (Matrix2x2.Columns(a.Item2 - a.Item1, b.Item2 - b.Item1).Inverse() is Matrix2x2 inverse)
        {
            return a.Item1 - ((inverse * (a.Item1 - b.Item1)).x * (a.Item2 - a.Item1));
        }
        else
        {
            return null;
        }
    }

    public static (Vector2, float)? ProjectOnPath(Vector2 vector, IEnumerable<Vector2> path)
    {
        return Utilities.MinBy(
            Utilities.Tuple(path)
                .Select(segment => ProjectOnLineSegment(vector, segment))
                .Select(projection => (projection, Vector2.Distance(vector, projection))),
            tuple => tuple.Item2);
    }

    public static Vector2 ProjectOnLineSegment(Vector2 vector, (Vector2 a, Vector2 b) segment)
    {
        Vector2 delta = segment.b - segment.a;
        float dot = Vector2.Dot(vector - segment.a, delta) / delta.sqrMagnitude;
        return segment.a + Mathf.Clamp01(dot) * delta;
    }

    /// <summary>
    /// TrimPath performs an inplace trim of path.
    /// All points along the path with a distance greater than length from its origin are removed.
    /// </summary>
    public static void TrimPath(List<Vector2> path, float length)
    {
        float accumulated = 0.0f;

        for (int i = 1; i < path.Count; i++)
        {
            accumulated += Vector2.Distance(path[i - 1], path[i]);

            if (accumulated > length)
            {
                float t = 1.0f - ((accumulated - length) / Vector2.Distance(path[i - 1], path[i]));
                path[i] = Vector2.Lerp(path[i - 1], path[i], t);

                if (i + 1 < path.Count)
                {
                    path.RemoveRange(i + 1, path.Count - i - 1);
                }

                break;
            }
        }
    }
}
