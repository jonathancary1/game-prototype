using System.Collections;
using UnityEngine;

/// <summary>
/// FloorTileManager manages a floor tile's animation.
/// Floor tiles are spawned and positioned by FloorManager.
/// </summary>
public class FloorTileManager : MonoBehaviour
{
    public float Duration;
    public Color Color;
    public Vector2 Size;

    private Coroutine coroutine;
    private SpriteRenderer spriteRenderer;

    public bool IsAnimating => coroutine != null;

    public Vector3? Position => IsAnimating ? spriteRenderer.transform.position : null as Vector3?;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = Size;
        spriteRenderer.color = Color.clear;
    }

    /// <summary>
    /// StartAnimation performs one cycle of floor tile animation at the given position.
    /// </summary>
    public void StartAnimation(Vector3 position)
    {
        if (!IsAnimating)
        {
            spriteRenderer.transform.position = position;
            coroutine = StartCoroutine(Animation());
        }
    }

    /// <summary>
    /// Animation animates the floor tile according to the Duration, Color, and Size properties.
    /// </summary>
    private IEnumerator Animation()
    {
        spriteRenderer.size = Vector2.zero;
        spriteRenderer.color = Color;
        yield return null;

        for (float time = 0.0f; time < Duration; time += Time.deltaTime)
        {
            float t = time / Duration;
            spriteRenderer.size = Vector2.Lerp(Vector2.zero, Size, 5.0f * t);
            spriteRenderer.color = Color.Lerp(Color, Color.clear, (5.0f / 4.0f * t) - (1.0f / 4.0f));
            yield return null;
        }

        spriteRenderer.size = Size;
        spriteRenderer.color = Color.clear;
        coroutine = null;
    }
}
