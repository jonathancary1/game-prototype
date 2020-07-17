using UnityEngine;

public struct Matrix2x2
{
    public float a;
    public float b;
    public float c;
    public float d;

    public Matrix2x2(float a, float b, float c, float d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    public static Matrix2x2 Columns(Vector2 a, Vector2 b)
    {
        return new Matrix2x2(a.x, b.x, a.y, b.y);
    }

    public static Matrix2x2 Rows(Vector2 a, Vector2 b)
    {
        return new Matrix2x2(a.x, a.y, b.x, b.y);
    }

    public float Determinant()
    {
        return (a * d) - (b * c);
    }

    public Matrix2x2? Inverse()
    {
        float determinant = Determinant();

        if (determinant == 0.0f)
        {
            return null;
        }
        else
        {
            return 1.0f / determinant * new Matrix2x2(d, -b, -c, a);
        }
    }

    public static Matrix2x2 operator *(float scalar, Matrix2x2 matrix)
    {
        return new Matrix2x2(
            scalar * matrix.a, scalar * matrix.b,
            scalar * matrix.c, scalar * matrix.d);
    }

    public static Vector2 operator *(Matrix2x2 matrix, Vector2 vector)
    {
        return new Vector2(
            matrix.a * vector.x + matrix.b * vector.y,
            matrix.c * vector.x + matrix.d * vector.y);
    }
}
