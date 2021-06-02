using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPossessable
{
	public MobController Controller { get; set; }

	public abstract bool SetPossessed(MobController controller);
}
