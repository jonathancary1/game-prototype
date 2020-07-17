using UnityEngine;

/// <summary>
/// IdleManager manages the play character's idle state.
/// After time has elapsed, the player character moves into an idle position.
/// </summary>
public class IdleManager : MonoBehaviour
{
    public float Delay;

    private enum State { IsIdle, IsNotIdle, WillBeIdle }

    private State state;
    
    private float time;

    private StanceManager stanceManager;

    private void Awake()
    {
        stanceManager = GetComponent<StanceManager>();
        stanceManager.Delegate = AnimationDelegate;
    }

    void Update()
    {
        if (state == State.IsNotIdle && Time.time - time > Delay)
        {
            if (!stanceManager.IsAnimating)
            {
                stanceManager.Idle();
                state = State.WillBeIdle;
            }
        }
    }

    /// <summary>
    /// AnimationDelegate is used by StanceManager.
    /// This delegate detects when the player character has finished an animation.
    /// </summary>
    private void AnimationDelegate(bool animating)
    {
        if (state == State.WillBeIdle && !animating)
        {
            state = State.IsIdle;
        }
        else if (state == State.IsIdle && animating)
        {
            state = State.IsNotIdle;
        }
        else if (state == State.IsNotIdle && !animating)
        {
            time = Time.time;
        }
    }
}
