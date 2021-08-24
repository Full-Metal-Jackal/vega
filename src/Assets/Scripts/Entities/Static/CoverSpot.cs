using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpot : Entity
{
	// Объект являющийся укрытием. Представляет собой "Щит" простреливаемый с внутренней стороны, но не со внешней.
	// Может быть уничтожено гранатой
	// Досутпен если: 1. Перекрывает обзор игрока
	//                2. Не занят другим Мобом
	//                3. Ближайший
	// Взаимодействет с Навмешем, создает/обязан находиться в зоне Cover.
	// 

    public bool IsOccupied;
	public bool IsDestroyed;
	public float radius = 2;
	//public bool IsSafe;  //Возможно лишнее


	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
			IsOccupied = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanTakeCover)
			IsOccupied = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
