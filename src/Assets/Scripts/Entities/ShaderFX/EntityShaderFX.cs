using System.Linq;
using UnityEngine;

public class EntityShaderFX : MonoBehaviour
{
	public Entity Entity { get; protected set; }
	public Renderer[] Renderers { get; protected set; }
	public Material[] Materials { get; protected set; }

	// Start is called before the first frame update
	protected virtual void Awake()
	{
		Entity = transform.parent.GetComponentInParent<Entity>();
		if (!Entity)
		{
			Debug.LogError($"No entity found for {this}. Mob's renderer FX should be two levels deeper than the mob itself.");
			return;
		}

		UpdateRenderers();
	}

	protected virtual void UpdateRenderers()
	{
		Renderers = Entity.GetComponentsInChildren<Renderer>();
		Materials = Renderers.Select((Renderer r) => r.material).ToArray();
	}
}
