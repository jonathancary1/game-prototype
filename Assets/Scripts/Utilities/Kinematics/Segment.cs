using UnityEngine;

public struct Segment
{
    public Vector3 Anchor;
    public Vector3 Target;
    public Vector3 Upwards;

    public Segment(Vector3 anchor, Vector3 target, Vector3 upwards)
    {
        Anchor = anchor;
        Target = target;
        Upwards = upwards;
    }

    public static (Segment, Segment) Pair((Vector3, Vector3, Vector3) values, Vector3 upwards)
    {
        return (new Segment(values.Item1, values.Item2, upwards), new Segment(values.Item2, values.Item3, upwards));
    }
}
