using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomShade : MonoBehaviour
{
	public bool IsExplored;
	[SerializeField] private Material exploredMat;
	[SerializeField] private Material unexploredMat;

	private void Start()
	{
		Renderer selectionRenderer;
		selectionRenderer = GetComponentInChildren<Renderer>();
		if (IsExplored)
		{
			selectionRenderer.material = exploredMat;
		}
		else
		{
			selectionRenderer.material = unexploredMat;
		}
	}
	public void changeState()
	{
		if (!IsExplored)
		{
			IsExplored = true;
			Renderer selectionRenderer;
			selectionRenderer = GetComponentInChildren<Renderer>();
			selectionRenderer.material = exploredMat;
		}
	}
}
