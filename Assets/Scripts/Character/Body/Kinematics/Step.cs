using UnityEngine;

public struct Step
{
    public Vector3 Position;
    public Vector3 Forward;

    public Step(Vector3 position, Vector3 forward)
    {
        Position = position;
        Forward = forward;
    }
}
