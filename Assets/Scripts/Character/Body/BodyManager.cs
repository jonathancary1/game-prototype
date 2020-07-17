using UnityEngine;

/// <summary>
/// BodyManager manages the player character's legs and head.
/// </summary>
public class BodyManager : MonoBehaviour
{
    public HeadManager Head;
    public LegManager Left;
    public LegManager Right;

    private HipsManager hipsManager;
    private StanceManager stanceManager;
    private StepManager stepManager;

    private void Awake()
    {
        hipsManager = GetComponent<HipsManager>();
        stanceManager = GetComponent<StanceManager>();
        stepManager = GetComponent<StepManager>();
    }

    public void SetBody()
    {
        Hips hips = hipsManager.Hips;
        Stance stance = stanceManager.Stance;
        Head.SetHead(hips, stance.Forward);
        Left.SetLeg(new Segment(hips.Left, stance.Left.Position, stance.Left.Forward), stepManager.Upwards);
        Right.SetLeg(new Segment(hips.Right, stance.Right.Position, stance.Right.Forward), stepManager.Upwards);
    }

    public void SetVisible(bool value)
    {
        Head.SetVisible(value);
        Left.SetVisible(value);
        Right.SetVisible(value);
    }
}
