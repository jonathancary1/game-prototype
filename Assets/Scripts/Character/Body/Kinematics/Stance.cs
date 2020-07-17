using UnityEngine;

public struct Stance
{
    public enum Foot { Left, Right }

    public Foot Pivot;

    public Step Left;
    public Step Right;

    public Vector3 Position => 0.5f * (Left.Position + Right.Position);

    public Vector3 Forward => Left.Forward + Right.Forward;

    public Stance(Step left, Step right, Foot pivot)
    {
        Pivot = pivot;
        Left = left;
        Right = right;
    }
}
