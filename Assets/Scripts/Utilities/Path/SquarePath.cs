using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquarePath
{
    /// <summary>
    /// Mesh returns a mesh of the path traced out by a square with side-length size along path.
    /// </summary>
    public static (Vector2[], int[]) Mesh(Vector2[] path, float size)
    {
        if (path.Length == 1)
        {
            Vector2 center = path[0];

            float delta = 0.5f * size;

            Vector2[] vertices = new Vector2[]
            {
                new Vector2(center.x - delta, center.y + delta),
                new Vector2(center.x + delta, center.y + delta),
                new Vector2(center.x - delta, center.y - delta),
                new Vector2(center.x + delta, center.y - delta)
            };

            int[] triangles = new int[] { 0, 1, 2, 3, 2, 1 };

            return (vertices, triangles);
        }
        else
        {
            return Join(Utilities.Tuple(path).Select(i => ConvexHull(i.Item1, i.Item2, size)));
        }
    }

    /// <summary>
    /// ConvexHull returns a mesh of the convex hull of two squares centered at a and b each with side-length size.
    /// </summary>
    private static (Vector2[], int[]) ConvexHull(Vector2 a, Vector2 b, float size)
    {
        if (a.x == b.x || a.y == b.y)
        {
            Vector2 center = 0.5f * (a + b);

            float dx = 0.5f * (size + Mathf.Abs(a.x - b.x));
            float dy = 0.5f * (size + Mathf.Abs(a.y - b.y));

            Vector2[] vertices = new Vector2[]
            {
                new Vector2(center.x - dx, center.y + dy),
                new Vector2(center.x + dx, center.y + dy),
                new Vector2(center.x - dx, center.y - dy),
                new Vector2(center.x + dx, center.y - dy)
            };

            int[] triangles = new int[] { 0, 1, 2, 3, 2, 1 };

            return (vertices, triangles);
        }

        float delta = 0.5f * size;

        if (b.x < a.x)
        {
            (a, b) = (b, a);
        }

        if (a.y < b.y)
        {
            Vector2[] vertices = new Vector2[]
            {
                new Vector2(a.x - delta, a.y + delta),
                new Vector2(a.x - delta, a.y - delta),
                new Vector2(a.x + delta, a.y - delta),
                new Vector2(b.x - delta, b.y + delta),
                new Vector2(b.x + delta, b.y + delta),
                new Vector2(b.x + delta, b.y - delta)
            };

            int[] triangles = new int[] { 0, 2, 1, 3, 4, 5, 0, 3, 2, 2, 3, 5 };

            return (vertices, triangles);
        }
        else
        {
            Vector2[] vertices = new Vector2[]
            {
                new Vector2(a.x - delta, a.y + delta),
                new Vector2(a.x + delta, a.y + delta),
                new Vector2(a.x - delta, a.y - delta),
                new Vector2(b.x + delta, b.y + delta),
                new Vector2(b.x - delta, b.y - delta),
                new Vector2(b.x + delta, b.y - delta)
            };

            int[] triangles = new int[] { 0, 1, 2, 3, 5, 4, 1, 3, 2, 2, 3, 4 };

            return (vertices, triangles);
        }
    }

    /// <summary>
    /// Join joins each mesh of meshes into a single mesh.
    /// </summary>
    private static (Vector2[], int[]) Join(IEnumerable<(Vector2[], int[])> meshes)
    {
        List<Vector2> vertices = new List<Vector2>();
        List<int> triangles = new List<int>();

        foreach ((Vector2[] vertices, int[] triangles) mesh in meshes)
        {
            triangles.AddRange(mesh.triangles.Select(i => i + vertices.Count));
            vertices.AddRange(mesh.vertices);
        }

        return (vertices.ToArray(), triangles.ToArray());
    }
}
