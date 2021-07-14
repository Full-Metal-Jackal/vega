using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T inst;
	public static T Instance
	{
		get
		{
			if (inst)
				return inst;

			Type type = typeof(T);
			inst = (T)FindObjectOfType(type, true);
			if (!inst)
				Debug.LogWarning($"В сцене нужен экземпляр {type}, но он отсутствует.");

			return inst;
		}
	}
}
