using UnityEngine;

public struct Hips
{
    public Vector3 Left;
    public Vector3 Right;

    public Vector3 Position => 0.5f * (Left + Right);

    public Hips(Vector3 left, Vector3 right)
    {
        Left = left;
        Right = right;
    }
}
