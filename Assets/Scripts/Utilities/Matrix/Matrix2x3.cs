using UnityEngine;

public struct Matrix2x3
{
    public float a;
    public float b;
    public float c;
    public float d;
    public float e;
    public float f;

    public Matrix2x3(float a, float b, float c, float d, float e, float f)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.e = e;
        this.f = f;
    }

    public static Matrix2x3 Columns(Vector2 a, Vector2 b, Vector2 c)
    {
        return new Matrix2x3(a.x, b.x, c.x, a.y, b.y, c.y);
    }

    public static Matrix2x3 Rows(Vector3 a, Vector3 b)
    {
        return new Matrix2x3(a.x, a.y, a.z, b.x, b.y, b.z);
    }

    public static Vector2 operator *(Matrix2x3 matrix, Vector3 vector)
    {
        return new Vector2(
            matrix.a * vector.x + matrix.b * vector.y + matrix.c * vector.z,
            matrix.d * vector.x + matrix.e * vector.y + matrix.f * vector.z);
    }
}
