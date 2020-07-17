using UnityEngine;

/// <summary>
/// StepManager provides functions for calculating stance and step positions.
/// </summary>
public class StepManager : MonoBehaviour
{
    public float Height;
    public float Splay;
    public float Spread;
    public float Strafe;
    public float Stride;

    public Vector3 Upwards;

    public delegate Step Lerp(Step from, Step to, float t);

    /// <summary>
    /// Idle returns an idle stance at the given position facing in the forward direction.
    /// </summary>
    public Stance Idle(Vector3 position, Vector3 forward, Stance.Foot pivot)
    {
        Quaternion rotation = Quaternion.LookRotation(forward, Upwards);

        Step left = new Step(
            position + (rotation * new Vector3(-0.5f * Spread, 0.0f, 0.0f)),
            rotation * Quaternion.Euler(0.0f, -Splay, 0.0f) * Vector3.forward);

        Step right = new Step(
            position + (rotation * new Vector3(0.5f * Spread, 0.0f, 0.0f)),
            rotation * Quaternion.Euler(0.0f, Splay, 0.0f) * Vector3.forward);

        return new Stance(left, right, pivot);
    }

    /// <summary>
    /// Idle returns the closest matching idle stance from the given stance parameter.
    /// </summary>
    public Step Idle(Stance stance)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            Quaternion rotation = Quaternion.Euler(0.0f, Splay, 0.0f);
            Vector3 forward = rotation * stance.Left.Forward;
            Vector3 direction = rotation * forward;
            Vector3 perpendicular = -Spread * Vector3.Cross(forward, Upwards).normalized;
            Vector3 position = stance.Left.Position + perpendicular;
            return new Step(position, direction);
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0.0f, -Splay, 0.0f);
            Vector3 forward = rotation * stance.Right.Forward;
            Vector3 direction = rotation * forward;
            Vector3 perpendicular = Spread * Vector3.Cross(forward, Upwards).normalized;
            Vector3 position = stance.Right.Position + perpendicular;
            return new Step(position, direction);
        }
    }

    /// <summary>
    /// Forward returns the position of the non-pivot foot after striding one step forward.
    /// The resulting player character's non-pivot foot's forward vector should equal or be as close as possible to forward.
    /// The non-pivot foot is constrained such that the player character's feet do not cross. 
    /// </summary>
    public Step Forward(Stance stance, Vector3 forward)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Left.Forward, forward, Upwards), -45.0f, 90.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha + Splay, Upwards) * stance.Left.Forward;
            Vector3 perpendicular = -Spread * Vector3.Cross(forward, Upwards).normalized;
            if (alpha < 0.0f)
            {
                float beta = Vector3.Angle(stance.Left.Forward + direction, forward);
                float magnitude = Mathf.Min(Stride, Spread / Mathf.Tan(Mathf.Deg2Rad * beta));
                Vector3 position = stance.Left.Position + perpendicular + (magnitude * forward.normalized);
                return new Step(position, direction);
            }
            else
            {
                Vector3 position = stance.Left.Position + perpendicular + (Stride * forward.normalized);
                return new Step(position, direction);
            }
        }
        else
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Right.Forward, forward, Upwards), -90.0f, 45.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha - Splay, Upwards) * stance.Right.Forward;
            Vector3 perpendicular = Spread * Vector3.Cross(forward, Upwards).normalized;
            if (alpha > 0.0f)
            {
                float beta = Vector3.Angle(direction + stance.Right.Forward, forward);
                float magnitude = Mathf.Min(Stride, Spread / Mathf.Tan(Mathf.Deg2Rad * beta));
                Vector3 position = stance.Right.Position + perpendicular + (magnitude * forward.normalized);
                return new Step(position, direction);
            }
            else
            {
                Vector3 position = stance.Right.Position + perpendicular + (Stride * forward.normalized);
                return new Step(position, direction);
            }
        }
    }

    /// <summary>
    /// Back returns the position of the non-pivot foot after striding one step backward.
    /// The resulting player character's non-pivot foot's forward vector should equal or be as close as possible to forward.
    /// The non-pivot foot is constrained such that the player character's feet do not cross. 
    /// </summary>
    public Step Back(Stance stance, Vector3 forward)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Left.Forward, forward, Upwards), -45.0f, 90.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha + Splay, Upwards) * stance.Left.Forward;
            Vector3 perpendicular = -Spread * Vector3.Cross(forward, Upwards).normalized;
            if (alpha > 0.0f)
            {
                float beta = Vector3.Angle(stance.Left.Forward + direction, forward);
                float magnitude = Mathf.Min(Stride, Spread / Mathf.Tan(Mathf.Deg2Rad * beta));
                Vector3 position = stance.Left.Position + perpendicular - (magnitude * forward.normalized);
                return new Step(position, direction);
            }
            else
            {
                Vector3 position = stance.Left.Position + perpendicular - (Stride * forward.normalized);
                return new Step(position, direction);
            }
        }
        else
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Right.Forward, forward, Upwards), -90.0f, 45.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha - Splay, Upwards) * stance.Right.Forward;
            Vector3 perpendicular = Spread * Vector3.Cross(forward, Upwards).normalized;
            if (alpha < 0.0f)
            {
                float beta = Vector3.Angle(direction + stance.Right.Forward, forward);
                float magnitude = Mathf.Min(Stride, Spread / Mathf.Tan(Mathf.Deg2Rad * beta));
                Vector3 position = stance.Right.Position + perpendicular - (magnitude * forward.normalized);
                return new Step(position, direction);
            }
            else
            {
                Vector3 position = stance.Right.Position + perpendicular - (Stride * forward.normalized);
                return new Step(position, direction);
            }
        }
    }

    /// <summary>
    /// Right returns the position of the non-pivot foot after strafing one step to the right.
    /// The resulting player character's non-pivot foot's forward vector should equal or be as close as possible to forward.
    /// </summary>
    public Step Right(Stance stance, Vector3 forward)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Left.Forward, forward, Upwards), -45.0f, 90.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha + Splay, Upwards) * stance.Left.Forward;
            Vector3 position = stance.Left.Position - (Strafe * Vector3.Cross(forward, Upwards).normalized);
            return new Step(position, direction);
        }
        else
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Right.Forward, forward, Upwards), -90.0f, 45.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha - Splay, Upwards) * stance.Right.Forward;
            Vector3 perpendicular = Vector3.Cross(forward, Upwards).normalized;
            float distance = Vector3.Distance(stance.Left.Position, stance.Right.Position);
            Vector3 position = stance.Right.Position + (Mathf.Min(Spread, distance) * perpendicular);
            return new Step(position, direction);
        }
    }

    /// <summary>
    /// Left returns the position of the non-pivot foot after strafing one step to the left.
    /// The resulting player character's non-pivot foot's forward vector should equal or be as close as possible to forward.
    /// </summary>
    public Step Left(Stance stance, Vector3 forward)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Left.Forward, forward, Upwards), -45.0f, 90.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha + Splay, Upwards) * stance.Left.Forward;
            Vector3 perpendicular = Vector3.Cross(forward, Upwards).normalized;
            float distance = Vector3.Distance(stance.Left.Position, stance.Right.Position);
            Vector3 position = stance.Left.Position - (Mathf.Min(Spread, distance) * perpendicular);
            return new Step(position, direction);
        }
        else
        {
            float alpha = Mathf.Clamp(Vector3.SignedAngle(stance.Right.Forward, forward, Upwards), -90.0f, 45.0f);
            Vector3 direction = Quaternion.AngleAxis(alpha - Splay, Upwards) * stance.Right.Forward;
            Vector3 position = stance.Right.Position + (Strafe * Vector3.Cross(forward, Upwards).normalized);
            return new Step(position, direction);
        }
    }

    /// <summary>
    /// StrafeLerp interpolates between two step values, creating a strafe animation.
    /// </summary>
    public Step StrafeLerp(Step from, Step to, float t)
    {
        float cross = Vector3.Cross((to.Position - from.Position).normalized, Upwards.normalized).magnitude;
        float distance = Mathf.Clamp01(Vector3.Distance(from.Position, to.Position) / Strafe);
        float parabola = Mathf.Clamp01(4.0f * t * (1.0f - t));
        Vector3 vector = cross * distance * parabola * Height * Upwards.normalized;
        Vector3 position = Vector3.Lerp(from.Position, to.Position, t) + vector;
        Vector3 forward = Vector3.Slerp(from.Forward, to.Forward, t);
        return new Step(position, forward);
    }

    /// <summary>
    /// StrideLerp interpolates between two step values, creating a stride animation.
    /// </summary>
    public Step StrideLerp(Step from, Step to, float t)
    {
        float cross = Vector3.Cross((to.Position - from.Position).normalized, Upwards.normalized).magnitude;
        float distance = Mathf.Clamp01(Vector3.Distance(from.Position, to.Position) / (2.0f * Stride));
        float parabola = Mathf.Clamp01(4.0f * t * (1.0f - t));
        Vector3 vector = cross * distance * parabola * Height * Upwards.normalized;
        Vector3 position = Vector3.Lerp(from.Position, to.Position, t) + vector;
        Vector3 forward = Vector3.Slerp(from.Forward, to.Forward, t);
        return new Step(position, forward);
    }
}
