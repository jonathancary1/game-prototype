using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// CharacterManager manages the player character's body and movement input.
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public CameraManager CameraManager;

    public bool AllowInput;

    public Vector3 Position => stanceManager.Stance.Position;

    public Vector3 Upwards => stepManager.Upwards;

    private BodyManager bodyManager;
    private StanceManager stanceManager;
    private StepManager stepManager;

    private void Awake()
    {
        bodyManager = GetComponent<BodyManager>();
        stanceManager = GetComponent<StanceManager>();
        stepManager = GetComponent<StepManager>();
    }

    private void Update()
    {
        if (AllowInput)
        {
            CaptureInput();
        }
    }

    /// <summary>
    /// CollidesWith returns true if the palyer character is sufficiently close to path.
    /// </summary>
    public bool CollidesWith(IEnumerable<Vector2> path, float size)
    {
        Vector3[] vectors = new Vector3[]
        {
            bodyManager.Left.Foot.Anchor,
            bodyManager.Left.Foot.Target,
            bodyManager.Right.Foot.Anchor,
            bodyManager.Right.Foot.Target
        };

        return vectors
            .Select(vector => Geometry.ProjectOnPath(new Vector2(vector.x, vector.z), path))
            .Where(projection => projection.HasValue)
            .Any(projection => projection.Value.Item2 < 0.5f * size);
    }

    /// <summary>
    /// SetCharacter sets the player character's mesh.
    /// Position, which is used by CameraManager, is updated.
    /// </summary>
    public void SetCharacter()
    {
        bodyManager.SetBody();
    }

    /// <summary>
    /// SetIdle sets the player character's mesh into the idle position.
    /// </summary>
    public void SetIdle()
    {
        stanceManager.Idle(Vector3.zero, Vector3.forward);
        SetCharacter();
    }

    /// <summary>
    /// SetVisible sets the player character's visibility.
    /// </summary>
    public void SetVisible(bool value)
    {
        bodyManager.SetVisible(value);
    }

    /// <summary>
    /// CaptureInput converts keyboard input into player character movement.
    /// The camera yaw is used for determining in which direction to move.
    /// </summary>
    private void CaptureInput()
    {
        if (stanceManager.IsAnimating)
        {
            return;
        }

        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) { direction += Vector3.forward; }
        if (Input.GetKey(KeyCode.A)) { direction += Vector3.left; }
        if (Input.GetKey(KeyCode.S)) { direction += Vector3.back; }
        if (Input.GetKey(KeyCode.D)) { direction += Vector3.right; }

        if (direction.z == 1.0f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + CameraManager.Yaw;
            Vector3 forward = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
            stanceManager.Forward(forward);
        }
        else if (direction.z == -1.0f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + CameraManager.Yaw + 180.0f;
            Vector3 forward = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
            stanceManager.Back(forward);
        }
        else if (direction.x == 1.0f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + CameraManager.Yaw - 90.0f;
            Vector3 forward = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
            stanceManager.Right(forward);
        }
        else if (direction.x == -1.0f)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up) + CameraManager.Yaw + 90.0f;
            Vector3 forward = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
            stanceManager.Left(forward);
        }
    }
}
