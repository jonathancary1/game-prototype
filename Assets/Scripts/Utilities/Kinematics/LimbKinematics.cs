using UnityEngine;

public class LimbKinematics
{
    /// <summary>
    /// Calculate solves for the joint position of a limb.
    /// The position and orientation of the limb are specified by segment.
    /// The first and second segments of the limb have lengths specified by lengths.
    /// </summary>
    public static (Vector3, Vector3, Vector3) Calculate(Segment segment, (float, float) lengths)
    {
        float distance = Mathf.Clamp(
            Vector3.Distance(segment.Anchor, segment.Target),
            lengths.Item1 - lengths.Item2,
            lengths.Item1 + lengths.Item2);
        Vector3 vector = segment.Target - segment.Anchor;
        Vector3 axis = Vector3.Cross(vector, segment.Upwards);
        float angle = Angle(distance, lengths.Item1, lengths.Item2);
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 midpoint = segment.Anchor + (lengths.Item1 * (rotation * vector.normalized));
        Vector3 target = segment.Anchor + (distance * vector.normalized);
        return (segment.Anchor, midpoint, target);
    }

    /// <summary>
    /// Angle returns the angle adjacent to side c in radians of the triangle abc. 
    /// </summary>
    private static float Angle(float a, float b, float c)
    {
        return Mathf.Rad2Deg * Mathf.Acos(((a * a) + (b * b) - (c * c)) / (2.0f * a * b));
    }
}
