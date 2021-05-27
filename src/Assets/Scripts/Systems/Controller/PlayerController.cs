using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MobController
{
    public CameraController camController;

    protected override bool Initialize()
	{
        if (!camController && !TryGetComponent(out camController))
            Debug.LogWarning($"Couldn't get camera controller for {this}.");
        return Initialized = base.Initialize();
    }

    public override bool PossessMob(Mob mob)
	{
        bool result = base.PossessMob(mob);
        if (result && camController)
            camController.SetTrackedMob(mob);
        return result;
	}

    protected override Vector3 GetMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);
        return movement;
    }
}
