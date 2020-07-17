using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// EnemyManager manages a single enemy instance.
/// After spawning, an enemy moves toward the player in random increments, tracing out a path.
/// SceneManager detects player collisions with enemy paths and inflicts damage accordingly. 
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public CharacterManager CharacterManager;

    public float Length;
    public float Size;
    public float Speed;

    private List<Vector2> path;

    private MeshFilter meshFilter;

    public Vector2[] Path => path.ToArray();

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Start()
    {
        StartCoroutine(EnemyLogic());
    }

    private void Update()
    {
        var (vertices, triangles) = SquarePath.Mesh(path.ToArray(), Size);

        meshFilter.mesh = new Mesh
        {
            vertices = vertices.Select(vertex => new Vector3(vertex.x, 0.0f, vertex.y)).ToArray(),
            triangles = triangles
        };
    }

    /// <summary>
    /// SetPosition is used by SceneManager in order to set an initial position after instantiation.
    /// </summary>
    public void SetPosition(Vector2 position)
    {
        path = new List<Vector2> { position };
    }

    /// <summary>
    /// EnemyLogic repeatedly moves the enemy toward the player along the best orthogonal direction for random lengths.
    /// </summary>
    private IEnumerator EnemyLogic()
    {
        Vector2[] directions = new Vector2[] { Vector2.right, Vector2.left, Vector2.up, Vector2.down };

        while (true)
        {
            Vector2 position = new Vector2(CharacterManager.Position.x, CharacterManager.Position.z);
            Vector2 target = (position - path[0]).normalized;
            Vector2 direction = Utilities.MinBy(directions, vector => Vector2.Angle(vector, target)).Value;
            float distance = Size * (1 << (2 * Random.Range(0, 4)));
            yield return Move(direction, distance, distance / Speed);
        }
    }

    /// <summary>
    /// Move lerps from the enemy's current position to a target as specified by the given parameters.
    /// </summary>
    private IEnumerator Move(Vector2 direction, float distance, float duration)
    {
        Vector2 position = path[0];
        Vector2 translation = distance * direction.normalized;

        path.Insert(0, position);

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            path[0] = position + t / duration * translation;
            Geometry.TrimPath(path, Length);
            yield return null;
        }

        path[0] = position + translation;
        yield return null;
    }
}
