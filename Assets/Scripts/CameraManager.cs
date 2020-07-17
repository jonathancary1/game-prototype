using UnityEngine;

/// <summary>
/// CameraManager manages mouse input and camera positioning.
/// </summary>
public class CameraManager : MonoBehaviour
{
    public CharacterManager CharacterManager;

    public Vector2 MouseSensitivity;

    public bool AllowInput;

    public float Yaw
    {
        get => yaw;
        set => yaw = Mathf.DeltaAngle(0.0f, value);
    }

    public float Pitch
    {
        get => pitch;
        set => pitch = Mathf.Clamp(value, 0.0f, 90.0f);
    }

    public float Distance;
    public float Height;

    private float yaw;
    private float pitch;

    private void Update()
    {
        if (AllowInput)
        {
            CaptureInput();
        }
    }

    private void CaptureInput()
    {
        Yaw += MouseSensitivity.x * Input.GetAxis("Mouse X");
        Pitch += MouseSensitivity.y * Input.GetAxis("Mouse Y");
    }

    public void SetCamera()
    {
        Vector3 position = CharacterManager.Position;
        Vector3 upwards = CharacterManager.Upwards.normalized;
        Vector3 projection = Vector3.ProjectOnPlane(position, upwards);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0.0f);
        Vector3 delta = Distance * (rotation * Vector3.back);
        transform.position = projection + (Height * upwards) + delta;
        transform.rotation = Quaternion.LookRotation(-delta, upwards);
    }
}
