using System.Linq;
using UnityEngine;

/// <summary>
/// FloorManager manages floor tiles, displaying them around the position of the player.
/// </summary>
public class FloorManager : MonoBehaviour
{
    public int Tiles;

    public Vector2 Range;
    public Vector2 Spacing;

    public float SpawnInterval;

    public CharacterManager CharacterManager;

    private FloorTileManager[] tiles;

    private void Awake()
    {
        GameObject tile = Resources.Load<GameObject>("Floor Tile");

        tiles = new FloorTileManager[Tiles];

        for (int i = 0; i < Tiles; i++)
        {
            tiles[i] = Instantiate(tile).GetComponent<FloorTileManager>();
        }
    }

    private void Start()
    {
        InvokeRepeating("SpawnTile", 0.0f, SpawnInterval);
    }

    /// <summary>
    /// SpawnTile spawns a floor tile at a random position nearby the player.
    /// </summary>
    private void SpawnTile()
    {
        foreach (FloorTileManager tile in tiles)
        {
            if (!tile.IsAnimating)
            {
                if (RandomPosition() is Vector3 position)
                {
                    tile.StartAnimation(position);
                }

                break;
            }
        }
    }

    /// <summary>
    /// RandomPosition returns an unused position for a floor tile.
    /// If no position could be found, null is returned.
    /// </summary>
    private Vector3? RandomPosition()
    {
        for (int i = 0; i < 16; i++)
        {

            float x = CharacterManager.Position.x + Random.Range(-Range.x, Range.x);
            float y = CharacterManager.Position.z + Random.Range(-Range.y, Range.y);

            Vector3 position = new Vector3(x - (x % Spacing.x), 0.2f * Random.Range(1, 6), y - (y % Spacing.y));

            if (tiles.All(tile => tile.Position != position))
            {
                return position;
            }
        }

        return null;
    }
}
