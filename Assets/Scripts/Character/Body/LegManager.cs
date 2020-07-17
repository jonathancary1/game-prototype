using System;
using UnityEngine;

/// <summary>
/// LegManager manages the player character's leg mesh.
/// </summary>
public class LegManager : MonoBehaviour
{
    public float FemurLength;
    public float TibiaLength;
    public float FootLength;

    public float Width;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public Segment Femur { get; private set; }

    public Segment Tibia { get; private set; }

    public Segment Foot { get; private set; }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// SetLeg sets the leg to match segment with the foot upwards direction facing upwards.
    /// The leg mesh is generated using LinearPath with Width determining its thickness.
    /// Inverse kinematics are used for determining the knee position. 
    /// </summary>
    public void SetLeg(Segment segment, Vector3 upwards)
    {
        segment.Target += Width * upwards.normalized;

        var (a, b, c) = LimbKinematics.Calculate(segment, (FemurLength, TibiaLength));

        Vector3 d = c - (0.5f * Width * upwards.normalized);
        Vector3 e = d + (FootLength * segment.Upwards.normalized);

        Femur = new Segment(a, b, segment.Upwards);
        Tibia = new Segment(b, c, segment.Upwards);
        Foot = new Segment(d, e, upwards);

        (Vector3, Vector3) basis = (segment.Target - segment.Anchor, segment.Upwards);
        Vector3.OrthoNormalize(ref basis.Item1, ref basis.Item2);
        Plane plane = new Plane(basis, segment.Anchor);
        Func<Vector3, Vector2> projection = plane.Projection();

        Vector2[] path = new Vector2[]
        {
            projection(a),
            projection(b),
            projection(c),
            projection(d),
            projection(e)
        };

        var (vertices, triangles) = LinearPath.Mesh(path, Width, plane, -0.5f * Width, 0.5f * Width);

        meshFilter.mesh = new Mesh { vertices = vertices, triangles = triangles };
    }

    public void SetVisible(bool value)
    {
        meshRenderer.enabled = value;
    }
}
