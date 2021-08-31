using System.Collections;
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
	public bool isSafe;  //Define if player can easily attack/see this cover spot
	public float radius = 2;
	private Mob currentUser;

	private void OnTriggerEnter(Collider other)
	{
		if (!isOccupied)
		{
			if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
				isOccupied = true;
			currentUser = mob;
		}	
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob))
		{
			if (currentUser == mob)
			{
				isOccupied = false;
				currentUser = null;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
