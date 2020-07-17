using UnityEngine;

/// <summary>
/// HeadManager manages the player character's head mesh.
/// </summary>
public class HeadManager : MonoBehaviour
{
    public Vector2 Offset;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// SetHead sets the player character's head position and rotation relative to the given hip value.
    /// </summary>
    public void SetHead(Hips hips, Vector3 forward)
    {
        Vector3 perpendicular = (hips.Right - hips.Left).normalized;
        Vector3 upwards = Vector3.Cross(forward.normalized, perpendicular);
        transform.position = hips.Position + (Offset.x * forward.normalized) + (Offset.y * upwards);
        transform.rotation = Quaternion.LookRotation(forward, upwards);
    }

    public void SetVisible(bool value)
    {
        meshRenderer.enabled = value;
    }
}
