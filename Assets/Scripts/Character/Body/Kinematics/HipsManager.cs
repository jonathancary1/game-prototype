using UnityEngine;

/// <summary>
/// HipsManager manages the player character's hip positions.
/// </summary>
public class HipsManager : MonoBehaviour
{
    public float Gait;
    public float Height;

    private StanceManager stanceManager;
    private StepManager stepManager;

    public Hips Hips
    {
        get
        {
            Vector3 position = stanceManager.Stance.Position;
            Quaternion rotation = Quaternion.LookRotation(stanceManager.Stance.Forward, stepManager.Upwards);

            Vector3 left = position + (rotation * new Vector3(-0.5f * stepManager.Spread, Height, 0.0f));
            Vector3 right = position + (rotation * new Vector3(0.5f * stepManager.Spread, Height, 0.0f));

            left += Gait * Vector3.Project(stanceManager.Stance.Left.Position - position, stepManager.Upwards);
            right += Gait * Vector3.Project(stanceManager.Stance.Right.Position - position, stepManager.Upwards);

            return new Hips(left, right);
        }
    }

    private void Awake()
    {
        stanceManager = GetComponent<StanceManager>();
        stepManager = GetComponent<StepManager>();
    }
}
