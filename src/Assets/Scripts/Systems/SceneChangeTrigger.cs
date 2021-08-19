using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
	[SerializeField]
	private string sceneName;
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.IsPlayer)
		{
			ChangeScene(sceneName);
		}
	}

	public void ChangeScene(string sceneName)
	{
		print(sceneName);
		LevelManager.Instance.LoadScene(sceneName);
	}
}
