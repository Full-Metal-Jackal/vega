using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStage : MonoBehaviour
{
	public virtual bool Available => false;
	public virtual bool Complete => false;
}
