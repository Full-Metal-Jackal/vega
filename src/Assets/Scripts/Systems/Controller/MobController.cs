using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    /// <summary>
    /// Tells the controller what mob should be possessed as the level starts.
    /// Needed only in specific cases to possess mobs in the editor where the controller can't be attached to the mob itself.
    /// </summary>
    public Mob possessAtStart;
    public Mob Possessed { get; protected set; }

    public bool Initialized { get; protected set; }

    protected int Id { get; private set; }

    private Vector3 Movement;

    public void Start()
    {
        Initialize();
    }

    protected virtual bool Initialize()
    {
        if ((possessAtStart is Mob mob) || TryGetComponent(out mob))
            PossessMob(mob);
        return Initialized = true;
    }

    public virtual bool PossessMob(Mob mob)
    {
        if (!(mob is IPossessable possessable))
            return false;
        possessable.SetPossessed(this);
        Possessed = mob;
        Debug.Log($"Controller {Id} possessed {mob}.");
        return true;
    }

    private void Update()
    {
        if (!Possessed)
            return;
        Movement = GetMovement();
    }

    protected virtual Vector3 GetMovement() => Vector3.zero;

    private void FixedUpdate()
    {
        Possessed.Move(Time.fixedDeltaTime, Movement);
    }
}
