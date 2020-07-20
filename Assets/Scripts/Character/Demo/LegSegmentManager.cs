using UnityEngine;

public class LegSegmentManager : MonoBehaviour
{
    public LegManager Leg;

    public Transform Anchor;
    public Transform Target;
    public Transform Forward;

    private void Update()
    {
        Vector3 anchor = Anchor.position;
        Vector3 target = Target.position;
        Vector3 relative = Forward.position - Vector3.Project(Forward.position, target - anchor);
        Vector3 upwards = Vector3.ProjectOnPlane(relative, Vector3.up);
        Leg.SetLeg(new Segment(anchor, target, upwards), Vector3.up);
    }
}
