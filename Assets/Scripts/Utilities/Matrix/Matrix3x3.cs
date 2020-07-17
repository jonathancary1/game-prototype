using UnityEngine;

public struct Matrix3x3
{
    public float a;
    public float b;
    public float c;
    public float d;
    public float e;
    public float f;
    public float g;
    public float h;
    public float i;

    public Matrix3x3(float a, float b, float c, float d, float e, float f, float g, float h, float i)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.e = e;
        this.f = f;
        this.g = g;
        this.h = h;
        this.i = i;
    }

    public static Matrix3x3 Columns(Vector3 a, Vector3 b, Vector3 c)
    {
        return new Matrix3x3(a.x, b.x, c.x, a.y, b.y, c.y, a.z, b.z, c.z);
    }

    public static Matrix3x3 Rows(Vector3 a, Vector3 b, Vector3 c)
    {
        return new Matrix3x3(a.x, a.y, a.z, b.x, b.y, b.z, c.x, c.y, c.z);
    }

    public float Determinant()
    {
        return (a * e * i) + (c * d * h) + (b * f * g) - (c * e * g) - (a * f * h) - (b * d * i);
    }

    public Matrix3x3? Inverse()
    {
        float A = (e * i) - (f * h);
        float B = (f * g) - (d * i);
        float C = (d * h) - (e * g);

        float determinant = (a * A) + (b * B) + (c * C);

        if (determinant == 0.0f)
        {
            return null;
        }

        float D = (c * h) - (b * i);
        float E = (a * i) - (c * g);
        float F = (b * g) - (a * h);
        float G = (b * f) - (c * e);
        float H = (c * d) - (a * f);
        float I = (a * e) - (b * d);

        return 1.0f / determinant * new Matrix3x3(A, D, G, B, E, H, C, F, I);
    }

    public static Matrix3x3 operator *(float scalar, Matrix3x3 matrix)
    {
        return new Matrix3x3(
            scalar * matrix.a, scalar * matrix.b, scalar * matrix.c,
            scalar * matrix.d, scalar * matrix.e, scalar * matrix.f,
            scalar * matrix.g, scalar * matrix.h, scalar * matrix.i);
    }

    public static Vector3 operator *(Matrix3x3 matrix, Vector3 vector)
    {
        return new Vector3(
            matrix.a * vector.x + matrix.b * vector.y + matrix.c * vector.z,
            matrix.d * vector.x + matrix.e * vector.y + matrix.f * vector.z,
            matrix.g * vector.x + matrix.h * vector.y + matrix.i * vector.z);
    }
}
