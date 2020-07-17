using System.Collections;
using UnityEngine;

/// <summary>
/// StanceManager manages the player character's stance and animations between stances.
/// </summary>
public class StanceManager : MonoBehaviour
{
    public delegate void AnimationDelegate(bool animating);

    public AnimationDelegate Delegate;

    public float MinimumAnimationTime;

    public float StrafeSpeed;
    public float StrideSpeed;

    private Coroutine coroutine;

    private Stance stance;

    private StepManager stepManager;

    public bool IsAnimating => coroutine != null;

    public Stance Stance => stance;

    private void Awake()
    {
        stepManager = GetComponent<StepManager>();
    }

    public void Idle(Vector3 position, Vector3 forward)
    {
        StopCoroutine();
        stance = stepManager.Idle(position, forward, Stance.Foot.Left);
    }

    public void Idle()
    {
        if (!IsAnimating)
        {
            Stride(stepManager.Idle(stance));
        }
    }

    public void Forward(Vector3 forward)
    {
        if (!IsAnimating)
        {
            Stride(stepManager.Forward(stance, forward));
        }
    }

    public void Back(Vector3 forward)
    {
        if (!IsAnimating)
        {
            Stride(stepManager.Back(stance, forward));
        }
    }

    public void Right(Vector3 forward)
    {
        if (!IsAnimating)
        {
            Strafe(stepManager.Right(stance, forward));
        }
    }

    public void Left(Vector3 forward)
    {
        if (!IsAnimating)
        {
            Strafe(stepManager.Left(stance, forward));
        }
    }

    private void Strafe(Step target)
    {
        StopCoroutine();

        if (stance.Pivot == Stance.Foot.Left)
        {
            float distance = Vector3.Distance(stance.Right.Position, target.Position);
            float duration = Mathf.Max(MinimumAnimationTime, distance / StrafeSpeed);
            coroutine = StartCoroutine(Animate(stance.Right, target, duration, stepManager.StrafeLerp));
        }
        else
        {
            float distance = Vector3.Distance(stance.Left.Position, target.Position);
            float duration = Mathf.Max(MinimumAnimationTime, distance / StrafeSpeed);
            coroutine = StartCoroutine(Animate(stance.Left, target, duration, stepManager.StrafeLerp));
        }
    }

    private void Stride(Step target)
    {
        StopCoroutine();

        if (stance.Pivot == Stance.Foot.Left)
        {
            float distance = Vector3.Distance(stance.Right.Position, target.Position);
            float duration = Mathf.Max(MinimumAnimationTime, distance / StrideSpeed);
            coroutine = StartCoroutine(Animate(stance.Right, target, duration, stepManager.StrideLerp));
        }
        else
        {
            float distance = Vector3.Distance(stance.Left.Position, target.Position);
            float duration = Mathf.Max(MinimumAnimationTime, distance / StrideSpeed);
            coroutine = StartCoroutine(Animate(stance.Left, target, duration, stepManager.StrideLerp));
        }
    }

    private void StopCoroutine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private IEnumerator Animate(Step from, Step to, float duration, StepManager.Lerp lerp)
    {
        if (stance.Pivot == Stance.Foot.Left)
        {
            stance.Right = from;
        }
        else
        {
            stance.Left = from;
        }

        Delegate?.Invoke(true);

        yield return null;

        for (float t = Time.deltaTime; t < duration; t += Time.deltaTime)
        {
            if (stance.Pivot == Stance.Foot.Left)
            {
                stance.Right = lerp(from, to, t / duration);
            }
            else
            {
                stance.Left = lerp(from, to, t / duration);
            }

            yield return null;
        }

        if (stance.Pivot == Stance.Foot.Left)
        {
            stance.Right = to;
            stance.Pivot = Stance.Foot.Right;
        }
        else
        {
            stance.Left = to;
            stance.Pivot = Stance.Foot.Left;
        }

        coroutine = null;

        Delegate?.Invoke(false);
    }
}
