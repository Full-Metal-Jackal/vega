using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShade : MonoBehaviour
{
	public bool IsExplored;
	[SerializeField] 
	private Material exploredMat;
	[SerializeField] 
	private Material unexploredMat;

	private void Start()
	{
		Renderer[] selectionRenderer;
		selectionRenderer = GetComponentsInChildren<Renderer>();
		if (IsExplored)
		{
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].material = exploredMat;
			}
		}
		else
		{
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].material = unexploredMat;
			}
		}
	}
	public void changeState()
	{
		if (!IsExplored)
		{
			IsExplored = true;
			Renderer[] selectionRenderer;
			selectionRenderer = GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].material = exploredMat;
			}
		}
	}
}
