﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpot : MonoBehaviour
{
	// Объект являющийся укрытием. Представляет собой "Щит" простреливаемый с внутренней стороны, но не со внешней.
	// Может быть уничтожено гранатой
	// Досутпен если: 1. Перекрывает обзор игрока
	//                2. Не занят другим Мобом
	//                3. Ближайший
	// Взаимодействет с Навмешем, создает/обязан находиться в зоне Cover.
	// 

	public bool isOccupied;
	public bool isDestroyed;
	public float radius = 2;

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
			isOccupied = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			CoverSpot mobsCover = mob.GetComponent<AI.AIManager>().currentCover;
			if (mobsCover == this)
			{
				isOccupied = false;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}