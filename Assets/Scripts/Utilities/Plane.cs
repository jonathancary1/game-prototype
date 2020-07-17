using System;
using UnityEngine;

/// <summary>
/// Plane represents an affine transformation of the XZ-plane.
/// </summary>
public struct Plane
{
    public (Vector3, Vector3) Basis;

    public Vector3 Origin;

    public Vector3 Normal => Vector3.Cross(Basis.Item1, Basis.Item2).normalized;

    public Plane((Vector3, Vector3) basis, Vector3 origin)
    {
        Basis = basis;
        Origin = origin;
    }

    /// <summary>
    /// OrthogonalProjection returns the same value as Projection under the assumption the basis vectors are orthogonal.
    /// </summary>
    public Vector2 OrthogonalProjection(Vector3 vector)
    {
        return new Vector2(
            Vector3.Dot(vector, Basis.Item1) / Basis.Item1.sqrMagnitude,
            Vector3.Dot(vector, Basis.Item2) / Basis.Item2.sqrMagnitude);
    }

    /// <summary>
    /// OrthoNormalProjection returns the same value as Projection under the assumption the basis vectors are orthonormal.
    /// </summary>
    public Vector2 OrthoNormalProjection(Vector3 vector)
    {
        return new Vector2(Vector3.Dot(vector, Basis.Item1), Vector3.Dot(vector, Basis.Item2));
    }

    /// <summary>
    /// Projection returns the point whose affine tranformation equals the projection of vector onto the plane.
    /// </summary>
    public Vector2? Projection(Vector3 vector)
    {
        Matrix3x3 matrix = Matrix3x3.Columns(Basis.Item1, Basis.Item2, Vector3.Cross(Basis.Item1, Basis.Item2));

        if (matrix.Inverse() is Matrix3x3 inverse)
        {
            Matrix2x3 projection = new Matrix2x3(inverse.a, inverse.b, inverse.c, inverse.d, inverse.e, inverse.f);
            return projection * (vector - Origin);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Projection returns an anoymous function for computing several projections.
    /// This avoids needlessly recomputing matrix inversions. 
    /// </summary>
    public Func<Vector3, Vector2> Projection()
    {
        Matrix3x3 matrix = Matrix3x3.Columns(Basis.Item1, Basis.Item2, Vector3.Cross(Basis.Item1, Basis.Item2));

        if (matrix.Inverse() is Matrix3x3 inverse)
        {
            Vector3 origin = Origin;
            Matrix2x3 projection = new Matrix2x3(inverse.a, inverse.b, inverse.c, inverse.d, inverse.e, inverse.f);
            return vector => projection * (vector - origin);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Applies the affine transformation represented by the plane to vector.
    /// </summary>
    public Vector3 Map(Vector2 vector)
    {
        return (vector.x * Basis.Item1) + (vector.y * Basis.Item2) + Origin;
    }
}
