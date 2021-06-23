using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T inst;

	public static T Instance
	{
		get
		{
			if (inst != null)
				return inst;

			inst = (T)FindObjectOfType(typeof(T));
			if (inst == null)
				Debug.LogWarning($"В сцене нужен экземпляр {typeof(T)}, но он отсутствует.");

			return inst;
		}
	}
}
